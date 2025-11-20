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
        private static void Postfix()
        {
            if (GamePatches.sort?.WasPressedBeforeTick() == true)
            {
                if (Game.menu_inventory.show)
                {
                    Game.menu_inventory.containerController.inventory.Sort();
                    if (Game.menu_inventory.chestPanel.show)
                    {
                        Game.menu_inventory.chestPanel.inventory.Sort();
                    }
                }
            }
        }
    }
}