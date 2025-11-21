using Allumeria;
using Allumeria.DataManagement.Translation;
using Allumeria.Settings;
using Allumeria.Input;
using Allumeria.Items.ItemTagTypes;
using HarmonyLib;

namespace QolTweaks.Patches;

[HarmonyPatch]
internal static class TranslatorPatches
{
    [HarmonyPatch(typeof(Translator), nameof(Translator.LoadTranslation))]
    public class Translator_LoadTranslation_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            string text = "en-AU";
            text = (string)GameSettings.current_language.GetValue();
            // FIXME: Requires mod folder to be exactly "QolTweaks," doesn't play nice with zip files
            // Also I suspect there's some kind of sneaky race condition going on since sometimes the game freezes at loading translations
            string path = Directory.GetCurrentDirectory() + "/mods/QolTweaks/res/translations/" + text + "/keys.txt";
            if (File.Exists(path))
            {
                string[] array = File.ReadAllLines(path);
                foreach (string text2 in array)
                {
                    if (text2 != "")
                    {
                        string text3 = text2.Split(' ')[0];
                        Translator.translationKey.TryAdd(text3, text2.Substring(text3.Length + 1));
                    }
                }
                Logger.Info($"Translation dictionary loaded for {text} with {Translator.translationKey.Count} values.");
            }
            // Item.TranslateNames();
            ItemTag.Translate();
            GameSettings.TranslateAll();
            // ControllerHintEntry.TranslateAll();
            InputManager.Translate();
            // Effect.TranslateAll();
            // CraftingStation.TranslateAll();
            // InGameHUD.TranslateTips();
        }
    }
}