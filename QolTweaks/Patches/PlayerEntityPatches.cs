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
    
    // Get a list of inventory slots that contain items matching a list of ids
    private static List<InventorySlot> GetSlots(int[] idsToMatch, bool hotbarOnly = false) {
        List<InventorySlot> result = new List<InventorySlot>();
        InventorySlot[] slots = World.player.inventory.inventory.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            if (hotbarOnly && !slot.hotbar) continue;
            if (slot.IsEmpty()) continue;
            if (idsToMatch.Contains(slot.itemStack.itemID)) {
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

    private static int[] torchLikes = {
        Item.throwable_torch.itemID,
        Item.throwable_glow_bean.itemID,
        Block.torch.item.itemID,
        Block.white_torch.item.itemID,
        Block.ice_torch.item.itemID,
        Block.ritual_torch.item.itemID
    };

    // TODO: Switch to doing this by tags and adding custom tags to these items?
    private static int[] healthPotions = {
        Item.health_potion.itemID,
        Item.weak_health_potion.itemID,
        Item.steak.itemID,
        Item.fish_fillet.itemID,
        Item.gormet_fish_fillet.itemID,
        Item.red_mushroom.itemID,
    };

    private static int[] buffPotions = {
        Item.speed_potion.itemID,
        Item.regen_potion.itemID,
        Item.armour_potion.itemID,
        Item.flight_potion.itemID,
        Item.mining_potion.itemID,
        Item.water_walking_potion.itemID,
        Item.crate_potion.itemID,
        Item.bagel.itemID,
        Item.everything_bagel.itemID,
        Item.blueberry.itemID,
        Item.tomato.itemID,
        Item.bread.itemID,
    };

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.PlaceAndDestroy))]
    private static class PlaceAndDestroyPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ChunkManager chunkManager)
        {
            if (World.player.punchDelay != 0) return;

            // Place Torch
            if (GamePatches.place_torch?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlots(torchLikes, hotbarOnly: true);
                if (slots.Count == 0) return;
                InventorySlot slot = slots[0]; // Get first torch
                Item item = slot.itemStack.GetItem();
                // Place or throw
                if (item.block != null) {
                    PlaceBlock(chunkManager, slot, item.block);
                } else {
                    UseItem(slot, item);
                }
            }
            
            // Quick Heal
            if (GamePatches.quick_heal?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlots(healthPotions);
                if (slots.Count == 0) return;
                InventorySlot slot = slots[0]; // Get first health potion
                Item item = slot.itemStack.GetItem();
                UseItem(slot, item);
            }
            
            // Quick Buff
            if (GamePatches.quick_buff?.WasPressedBeforeTick() == true) {
                List<InventorySlot> slots = GetSlots(buffPotions);
                if (slots.Count == 0) return;
                foreach (InventorySlot slot in slots) {
                    Item item = slot.itemStack.GetItem();
                    UseItem(slot, item);
                }
            }
        }
    }
}