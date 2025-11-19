using Allumeria;
using Allumeria.Input;
using Allumeria.EntitySystem.Entities;
using Allumeria.ChunkManagement;
using HarmonyLib;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class PlayerEntityPatches
{
    public static InputChannel quick_stack_nearby = new InputChannel("quick_stack_nearby", Keys.G);

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Tick))]
    private static class TickPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (!World.player.dead && !World.player.inMenu)
            {
                if (quick_stack_nearby.WasPressedBeforeTick()) {
                    World.player.QuickStackToNearbyChests(Game.worldManager.world);
                }
            }
        }
    }
}