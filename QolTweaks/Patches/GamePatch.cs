using Allumeria;
using Allumeria.Input;
using HarmonyLib;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace QolTweaks.Patches;

[HarmonyPatch]
internal static class GamePatches
{
    public static InputChannel? quick_stack_nearby;
    public static InputChannel? sort;
    public static InputChannel? place_torch;

    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch("OnLoad")]
    public class Game_OnLoad_Patch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            quick_stack_nearby = new InputChannel("quick_stack_nearby", Keys.G);
            sort = new InputChannel("sort", MouseButton.Button3);
            place_torch = new InputChannel("place_torch", Keys.F);
        }
    }
}