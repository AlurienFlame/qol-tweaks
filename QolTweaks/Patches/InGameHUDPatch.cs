using Allumeria;
using Allumeria.ChunkManagement;
using Allumeria.UI;
using Allumeria.UI.Menus;
using Allumeria.UI.Text;
using HarmonyLib;
using QolTweaks.Patches;

[HarmonyPatch]
internal static class InGameHUDPatch
{
    [HarmonyPatch(typeof(InGameHUD), nameof(InGameHUD.Render))]
    public class InGameHUD_Render_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (GamePatches.enable_waila.value && World.player != null && !World.player.inMenu && World.player.isLookingAtBlock && World.lookingAtBlock != null && Game.worldManager.world.bossEntity == null)
            {
                string text = World.lookingAtBlock.item.translatedName;
                TextRenderer.DrawTextShadow(
                    text,
                    UIManager.scaledWidth / 2 - TextRenderer.GetTextWidth(text) / 2,
                    10,
                    UIManager.scale,
                    applyScaleToPos: true
                );
            }
        }
    }
}