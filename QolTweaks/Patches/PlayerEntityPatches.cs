using Allumeria;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
using Allumeria.Audio;
using Allumeria.Blocks.Blocks;
using Allumeria.Items;
using Allumeria.Items.ItemTagTypes;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using Allumeria.UI;
using Allumeria.UI.Text;
using Allumeria.Rendering;

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
    
    // Get a list of inventory slots that contain items matching a tag
    private static List<InventorySlot> GetSlotsWithTag(ItemTag tag, bool hotbarOnly = false) {
        List<InventorySlot> result = new List<InventorySlot>();
        InventorySlot[] slots = World.player.inventory.inventory.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            if (hotbarOnly && !slot.hotbar) continue;
            if (slot.IsEmpty()) continue;
            if (slot.itemStack.GetItem().HasTag(tag)) {
                result.Add(slot);
            }
        }
        return result;
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
        if (!item.CanConsume(World.player)) return;
        item.OnUse(World.player, Game.worldManager.world);
        if (item.GetTag(ItemTag.can_consume, out var _))
        {
            if (!PlayerEntity.allowInfiniteItems)
            {
                slot.DecrementItem();
            }
        }
    }
    
    private static void TryPlaceOrThrowTorch(PlayerEntity __instance, ChunkManager chunkManager) {
        int x = __instance.targetedBlockPlacePos.X;
        int y = __instance.targetedBlockPlacePos.Y;
        int z = __instance.targetedBlockPlacePos.Z;
        List<InventorySlot> torchSlots = GetSlotsWithTag(GamePatches.qoltweaks_torch, hotbarOnly: true);
        if (torchSlots.Count > 0) {
            InventorySlot slot = torchSlots[0];
            Block block = slot.itemStack.GetItem().block;
            if (__instance.isLookingAtBlock) {
                PlaceBlock(chunkManager, slot, block);
                return;
            }
        }
        List<InventorySlot> throwableTorchSlots = GetSlotsWithTag(GamePatches.qoltweaks_throwable_torch, hotbarOnly: true);
        if (throwableTorchSlots.Count > 0) {
            InventorySlot slot = throwableTorchSlots[0];
            Item item = slot.itemStack.GetItem();
            UseItem(slot, item);
        }
    }
    
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.PlaceAndDestroy))]
    private static class PlaceAndDestroyPatch
    {
        [HarmonyPostfix]
        private static void Postfix(PlayerEntity __instance, ChunkManager chunkManager)
        {
            if (World.player.punchDelay != 0) return;

            // Place Torch
            if (GamePatches.place_torch?.WasPressedBeforeTick() == true) {
                TryPlaceOrThrowTorch(__instance, chunkManager);
            }
            
            // Quick Heal
            if (GamePatches.quick_heal?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlotsWithTag(GamePatches.qoltweaks_health_potion);
                if (slots.Count > 0) {
                    InventorySlot slot = slots[0];
                    Item item = slot.itemStack.GetItem();
                    UseItem(slot, item);
                }
            }
            
            // Quick Buff
            if (GamePatches.quick_buff?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlotsWithTag(GamePatches.qoltweaks_buff_potion);
                if (slots.Count > 0) {
                    foreach (InventorySlot slot in slots) {
                        Item item = slot.itemStack.GetItem();
                        UseItem(slot, item);
                        
                    }
                }
            }
            
            // Recall
            if (GamePatches.recall?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlotsWithTag(GamePatches.qoltweaks_recall);
                if (slots.Count > 0) {
                    InventorySlot slot = slots[0];
                    Item item = slot.itemStack.GetItem();
                    UseItem(slot, item);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.GiveItem))]
        private static class GiveItemPatch
        {
            [HarmonyPostfix]
            private static void Postfix(PlayerEntity __instance, ItemStack stack)
            {
                InGameHUDPatch.recentlyPickedUpItems.Add(stack);
            }
        }
    }
}