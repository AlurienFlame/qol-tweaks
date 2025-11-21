using Allumeria;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.Rendering;
using Allumeria.UI;
using Allumeria.UI.Menus;
using Allumeria.UI.Text;
using HarmonyLib;
using OpenTK.Mathematics;
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
            if (GamePatches.enable_waila.value && World.player != null && !World.player.inMenu && Game.worldManager.world.bossEntity == null)
            {
                string text = "";
                bool didFindEntity = Game.worldManager.world.chunkManager.EntityRaycast(
                    World.player,
                    5.5f, // player reach is a magic number :(
                    Game.camera.position,
                    Game.camera.front,
                    out Entity outEntity,
                    10
                );
                bool didFindBlock = World.player.isLookingAtBlock;
                // Get block or entity name
                if (didFindEntity && didFindBlock)
                {
                    // find closest
                    float entityDist = (outEntity.position - Game.camera.position).Length;
                    float blockDist = (World.raycastHitPositionPrecise - Game.camera.position).Length;

                    // Matrix4 rotationMatrix = Matrix4.CreateScale(0.25f + World.player.selectScale);
                    // Drawing.debugCube.Draw(WorldRenderer.blockEntityShader, Drawing.defaultTextureNoMipMap, outEntity.position, rotationMatrix);
                    // Drawing.debugCube.Draw(WorldRenderer.blockEntityShader, Drawing.defaultTextureNoMipMap, World.raycastHitPositionPrecise, rotationMatrix);

                    if (entityDist <= blockDist)
                    {
                        text = outEntity.GetType().Name;
                    }
                    else
                    {
                        text = World.lookingAtBlock.item.translatedName;
                    }
                } else if ( didFindEntity )
                {
                    text = outEntity.GetType().Name;
                } else if ( World.player.isLookingAtBlock && World.lookingAtBlock != null )
                {
                    text = World.lookingAtBlock.item.translatedName;
                }
                
                
                // Render text
                if (text != "")
                {
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
}