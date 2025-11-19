using Allumeria;
using Allumeria.EntitySystem.Entities;
using HarmonyLib;


namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class PlayerEntityPatches
{
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Tick))]
    private static class BuildMenuPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            Logger.Info("Hello from PlayerEntity!");
        }
    }
}