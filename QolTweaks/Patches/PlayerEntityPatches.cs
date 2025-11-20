using Allumeria;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
using Allumeria.Audio;
using Allumeria.Blocks.Blocks;
using Allumeria.Items;
using Allumeria.Items.ItemTagTypes;
using HarmonyLib;
using System.Linq;

namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class PlayerEntityPatches
{
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Tick))]
    private static class TickPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (!World.player.dead && !World.player.inMenu)
            {
                if (GamePatches.quick_stack_nearby?.WasPressedBeforeTick() == true) {
                    World.player.QuickStackToNearbyChests(Game.worldManager.world);
                }
            }
        }
    }
    
    private static InventorySlot GetTorchSlot() {
        int[] torchLikes = {
            Item.throwable_torch.itemID,
            Item.throwable_glow_bean.itemID,
            Block.torch.item.itemID,
            Block.white_torch.item.itemID,
            Block.ice_torch.item.itemID,
            Block.ritual_torch.item.itemID
        };
        // Find the first torch-like item in the hotbar
        for (int i = 0; i < World.player.inventory.inventory.slots.Length; i++)
        {
            InventorySlot slot = World.player.inventory.inventory.slots[i];
            if (!slot.hotbar || slot.IsEmpty()) {
                continue;
            }
            if (torchLikes.Contains(slot.itemStack.itemID)) {
                return slot;
            }
        }
        return null;
    }
    
    private static void PlaceBlock(ChunkManager chunkManager, InventorySlot slot, Block block) {
        int x = World.player.targetedBlockPlacePos.X;
        int y = World.player.targetedBlockPlacePos.Y;
        int z = World.player.targetedBlockPlacePos.Z;
        if (block.IsValidLocation(x, y, z, World.player, Game.worldManager.world))
        {
            AudioPlayer.PlaySoundWorldRandom(block.blockMaterial.placeSound, World.player.targetedBlockPlacePos);
            chunkManager.SetBlockWithUpdateAndLight(x, y, z, block, block.GetPlaceMetadata(World.player, x, y, z, Game.worldManager.world), maintainFluid: true);
            chunkManager.MarkNeighboursDirty(x, y, z);
            chunkManager.GetBlock(x, y, z).OnPlace(World.player, x, y, z, Game.worldManager.world);
            chunkManager.RequestChunkFromCoords(x, y, z).preserveForUpgrade = true;
            World.player.punchDelay = World.player.defaultBuildDelay;
            World.player.currentChunk.preserveForUpgrade = true;
            if (!PlayerEntity.allowInfiniteItems)
            {
                slot.DecrementItem();
            }
        }
    }
    
    private static void UseItem(InventorySlot slot, Item item) {
        item.OnUse(World.player, Game.worldManager.world);
        if (item.GetTag(ItemTag.can_consume, out var _))
        {
            if (!PlayerEntity.allowInfiniteItems)
            {
                slot.DecrementItem();
            }
        }
    }
    
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.PlaceAndDestroy))]
    private static class PlaceAndDestroyPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ChunkManager chunkManager)
        {
            if (GamePatches.place_torch?.WasPressedBeforeTick() == true && World.player.punchDelay == 0) {
                InventorySlot slot = GetTorchSlot();
                if (slot == null) {
                    return;
                }
                // Figure out if we're dealing with a placable or a throwable
                Item item = slot.itemStack.GetItem();
                if (item.block != null) {
                    PlaceBlock(chunkManager, slot, item.block);
                } else if (item.CanConsume(World.player)) {
                    UseItem(slot, item);
                }
            }
        }
    }
}