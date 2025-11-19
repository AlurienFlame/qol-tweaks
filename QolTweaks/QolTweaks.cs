using Allumeria;
using Allumeria.Input; // for InputChannel
using HarmonyLib;
using Ignitron.Loader;
using OpenTK.Windowing.GraphicsLibraryFramework; // for MouseButton

namespace QolTweaks;

public sealed class QolTweaks : IModEntrypoint
{
    public static InputChannel sort = new InputChannel("sort", MouseButton.Button3);

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
}