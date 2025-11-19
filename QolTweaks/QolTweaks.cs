using Allumeria;
using Allumeria.Input;
using HarmonyLib;
using Ignitron.Loader;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace QolTweaks;

public sealed class QolTweaks : IModEntrypoint
{
    // FIXME: missing translation text, or any text at all
    public static InputChannel quick_stack_nearby;
    public static InputChannel sort;

    public void Main(ModBox box)
    {
        // this will be useful when writing transpiler
#if DEBUG
        Harmony.DEBUG = true;
#endif
        // https://harmony.pardeike.net/articles/intro.html
        // initialise harmony
        Harmony harmony = new("io.github.alurienflame.QolTweaks");
        harmony.PatchAll();
    }
    
    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch("OnLoad")]
    public class Game_OnLoad_Patch
    {
        [HarmonyPrefix]

        public static void Prefix()
        {
            quick_stack_nearby = new InputChannel("quick_stack_nearby", Keys.G);
            sort = new InputChannel("sort", MouseButton.Button3);
        }
    }
}
