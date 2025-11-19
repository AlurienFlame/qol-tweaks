using Allumeria;
using Allumeria.UI.Menus;
using HarmonyLib;

namespace ExampleMod.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.BuildMenu))]
    private static class BuildMenuPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            Logger.Info("Hello from example mod!");
        }
    }
}