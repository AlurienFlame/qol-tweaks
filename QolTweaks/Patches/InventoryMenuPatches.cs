using Allumeria;
using Allumeria.UI.Menus;
using HarmonyLib;

namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class InventoryMenuPatches
{
    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.Update))]
    private static class UpdatePatch
    {
        [HarmonyPostfix]
        private static void Postfix(InventoryMenu __instance)
        {
            if (GamePatches.sort?.IsDown() == true)
            {
                if (__instance.show)
                {
                    __instance.containerController.inventory.Sort();
                    if (__instance.chestPanel.show)
                    {
                        __instance.chestPanel.inventory.Sort();
                    }
                }
            }
        }
    }
}