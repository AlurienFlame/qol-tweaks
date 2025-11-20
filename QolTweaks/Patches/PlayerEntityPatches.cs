using Allumeria;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
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
        private static void Postfix()
        {
            if (GamePatches.place_torch.WasPressedBeforeTick() && World.player.punchDelay == 0) {
                Logger.Info("Placing torch.");
            }
        }
    }
}