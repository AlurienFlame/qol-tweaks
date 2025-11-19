using Allumeria;
using HarmonyLib;
using Ignitron.Loader;

namespace QolTweaks;

public sealed class QolTweaks : IModEntrypoint
{

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
        // TODO: Quick stack to nearby chests on hotkey. probably extend PlayerEntity input
        /*
        if (btn_quickStack.WasActivatedPrimary() && World.player != null)
        {
            World.player.QuickStackToNearbyChests(Game.worldManager.world);
        }
        */
    }
}