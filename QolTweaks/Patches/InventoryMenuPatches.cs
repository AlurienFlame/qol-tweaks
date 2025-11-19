using Allumeria;
using Allumeria.UI.Menus;
using Allumeria.Input;
using HarmonyLib;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class InventoryMenuPatches
{
    public static InputChannel sort = new InputChannel("sort", MouseButton.Button3);
    
    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.Update))]
    private static class UpdatePatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (sort.WasPressedBeforeTick())
            {
                if (Game.menu_inventory.show) {
                    Logger.Info("Sorting due to mouse wheel press.");
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