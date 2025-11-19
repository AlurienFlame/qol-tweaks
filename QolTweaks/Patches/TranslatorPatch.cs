using Allumeria;
using Allumeria.DataManagement.Translation;
using Allumeria.Settings;
using Allumeria.Input;
using HarmonyLib;

namespace QolTweaks.Patches;

[HarmonyPatch]
internal static class TranslatorPatches
{
    [HarmonyPatch(typeof(Translator))]
    [HarmonyPatch("LoadTranslation")]
    public class Translator_LoadTranslation_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            string text = "en-AU";
            text = (string)GameSettings.current_language.GetValue();
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
            InputManager.Translate();
        }
    }
}