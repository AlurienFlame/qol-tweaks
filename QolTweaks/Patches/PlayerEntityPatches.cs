using Allumeria;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
using Allumeria.Audio;
using Allumeria.Blocks.Blocks;
using Allumeria.Items;
using HarmonyLib;

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
    
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.PlaceAndDestroy))]
    private static class PlaceAndDestroyPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ChunkManager chunkManager)
        {
            if (GamePatches.place_torch.WasPressedBeforeTick() && World.player.punchDelay == 0) {
                // Search hotbar for torch, sticky torch, and other variants
                InventorySlot inventorySlot = World.player.inventory.inventory.slots[0];
                if (inventorySlot.IsEmpty()) {
                    return;
                }
                Block torchBlock = inventorySlot.itemStack.GetItem().block;

                // Place or throw the item in the accquired inventory slot
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