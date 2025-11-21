using Allumeria;
using Allumeria.Input;
using HarmonyLib;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Allumeria.Items;
using Allumeria.Items.ItemTagTypes;
using Allumeria.Blocks.Blocks;
using Allumeria.Settings;

namespace QolTweaks.Patches;

[HarmonyPatch]
internal static class GamePatches
{
    public static InputChannel? quick_stack_nearby;
    public static InputChannel? sort;
    public static InputChannel? place_torch;
    public static InputChannel? quick_heal;
    public static InputChannel? quick_buff;
    public static InputChannel? recall;

    public static ItemTag qoltweaks_torch = new ItemTag("qoltweaks_torch");
    public static ItemTag qoltweaks_throwable_torch = new ItemTag("qoltweaks_throwable_torch");
    public static ItemTag qoltweaks_health_potion = new ItemTag("qoltweaks_health_potion");
    public static ItemTag qoltweaks_buff_potion = new ItemTag("qoltweaks_buff_potion");
    public static ItemTag qoltweaks_recall = new ItemTag("qoltweaks_recall");

    public static SettingsEntryBool enable_waila = new SettingsEntryBool("enable_waila", SettingsCategory.misc, defaultValue: true);
    public static SettingsEntryBool enable_pickup_notifier = new SettingsEntryBool("enable_pickup_notifier", SettingsCategory.misc, defaultValue: true);

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
            quick_heal = new InputChannel("quick_heal", Keys.H);
            quick_buff = new InputChannel("quick_buff", Keys.B);
            recall = new InputChannel("recall", Keys.Y);

            Block.torch.item.AddTag(qoltweaks_torch);
            Block.white_torch.item.AddTag(qoltweaks_torch);
            Block.ice_torch.item.AddTag(qoltweaks_torch);
            Block.ritual_torch.item.AddTag(qoltweaks_torch);

            Item.throwable_torch.AddTag(qoltweaks_throwable_torch);
            Item.throwable_glow_bean.AddTag(qoltweaks_throwable_torch);

            Item.health_potion.AddTag(qoltweaks_health_potion);
            Item.weak_health_potion.AddTag(qoltweaks_health_potion);
            Item.steak.AddTag(qoltweaks_health_potion);
            Item.fish_fillet.AddTag(qoltweaks_health_potion);
            Item.gormet_fish_fillet.AddTag(qoltweaks_health_potion);
            Item.red_mushroom.AddTag(qoltweaks_health_potion);

            Item.speed_potion.AddTag(qoltweaks_buff_potion);
            Item.regen_potion.AddTag(qoltweaks_buff_potion);
            Item.armour_potion.AddTag(qoltweaks_buff_potion);
            Item.flight_potion.AddTag(qoltweaks_buff_potion);
            Item.mining_potion.AddTag(qoltweaks_buff_potion);
            Item.water_walking_potion.AddTag(qoltweaks_buff_potion);
            Item.crate_potion.AddTag(qoltweaks_buff_potion);
            Item.bagel.AddTag(qoltweaks_buff_potion);
            Item.everything_bagel.AddTag(qoltweaks_buff_potion);
            Item.blueberry.AddTag(qoltweaks_buff_potion);
            Item.tomato.AddTag(qoltweaks_buff_potion);
            Item.bread.AddTag(qoltweaks_buff_potion);

            Item.respawn_potion.AddTag(qoltweaks_recall);
        }
    }
}