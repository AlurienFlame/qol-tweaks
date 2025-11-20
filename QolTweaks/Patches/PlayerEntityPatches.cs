using Allumeria;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
using Allumeria.Audio;
using Allumeria.Blocks.Blocks;
using Allumeria.Items;
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
                if (GamePatches.quick_stack_nearby.WasPressedBeforeTick()) {
                    World.player.QuickStackToNearbyChests(Game.worldManager.world);
                }
            }
        }
    }
    
    private static int GetTorchIndex() {
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
            if (World.player.inventory.inventory.slots[i].IsEmpty()) {
                continue;
            }
            if (torchLikes.Contains(World.player.inventory.inventory.slots[i].itemStack.itemID)) {
                return i;
            }
        }
        return -1;
    }
    
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.PlaceAndDestroy))]
    private static class PlaceAndDestroyPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ChunkManager chunkManager)
        {
            if (GamePatches.place_torch.WasPressedBeforeTick() && World.player.punchDelay == 0) {
                int torchIdx = GetTorchIndex();
                if (torchIdx == -1) {
                    return;
                }
                // Get inventory slot at that index
                InventorySlot inventorySlot = World.player.inventory.inventory.slots[torchIdx];
                // Figure out if we're dealing with a placable or a throwable
                // TODO
                Block torchBlock = inventorySlot.itemStack.GetItem().block;

                // Throw it
                // TODO
                // Place it
                Logger.Info("Placing torch.");
                int x = World.player.targetedBlockPlacePos.X;
                int y = World.player.targetedBlockPlacePos.Y;
                int z = World.player.targetedBlockPlacePos.Z;
                if (torchBlock.IsValidLocation(x, y, z, World.player, Game.worldManager.world))
                {
                    AudioPlayer.PlaySoundWorldRandom(torchBlock.blockMaterial.placeSound, World.player.targetedBlockPlacePos);
                    chunkManager.SetBlockWithUpdateAndLight(x, y, z, torchBlock, torchBlock.GetPlaceMetadata(World.player, x, y, z, Game.worldManager.world), maintainFluid: true);
                    chunkManager.MarkNeighboursDirty(x, y, z);
                    chunkManager.GetBlock(x, y, z).OnPlace(World.player, x, y, z, Game.worldManager.world);
                    chunkManager.RequestChunkFromCoords(x, y, z).preserveForUpgrade = true;
                    World.player.punchDelay = World.player.defaultBuildDelay;
                    World.player.currentChunk.preserveForUpgrade = true;
                    if (!PlayerEntity.allowInfiniteItems)
                    {
                        inventorySlot.DecrementItem();
                    }
                }
            }
        }
    }
}