using Allumeria;
using HarmonyLib;
using Ignitron.Loader;

namespace ExampleMod;

public sealed class ExampleMod : IModEntrypoint
{
    public void Main(ModBox box)
    {
        // this will be useful when writing transpiler
#if DEBUG
        Harmony.DEBUG = true;
#endif

        // https://harmony.pardeike.net/articles/intro.html
        // initialise harmony
        Harmony harmony = new("io.github.danilwhale.ExampleMod");
        harmony.PatchAll();

        Logger.Info("Hello, World!");
    }
}