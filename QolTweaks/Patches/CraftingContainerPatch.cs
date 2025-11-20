using Allumeria;
using Allumeria.UI.Containers;
using Allumeria.Items;
using Allumeria.Input;
using HarmonyLib;

namespace QolTweaks.Patches;

// https://harmony.pardeike.net/articles/intro.html
[HarmonyPatch]
internal static class CraftingContainerPatches
{
    [HarmonyPatch(typeof(CraftingContainer), nameof(CraftingContainer.AttemptCraft))]
    private static class AttemptCraftPatch
    {
        [HarmonyPrefix]
        private static void Prefix(CraftingContainer __instance, ref bool ___refreshQueued, RecipeSlot recipeSlot, UISlot slot)
        {
            if (!slot.IsEmpty() && InputManager.placeModifier.IsDown() && !InputManager.ui_secondary.IsDown()) {
                int successes = 0;
                while (recipeSlot.recipe.TryCraft(__instance.inventoryAbstraction, out ItemStack resultingItem)) {
                    __instance.playerInventory.TryAddItem(resultingItem, out ItemStack _);
                    ___refreshQueued = true;
                    __instance.Refresh();
                    successes++;
                }
                if (successes > 0) {
                    __instance.PlayShuffleSound();
                }
            }
        }
    }
}