using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TheQueenOfSauceReminder;

public class ModEntry : Mod
{
    /// <summary>The mod configuration from the player.</summary>
    private ModConfig? _config;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        _config = Helper.ReadConfig<ModConfig>();
        helper.Events.GameLoop.DayStarted += DayStarted;
    }

    /// <summary>Raised after the days starts.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void DayStarted(object? sender, DayStartedEventArgs e)
    {
        _config ??= new ModConfig();
        var today = SDate.Now().DayOfWeek;
        if (Game1.stats.DaysPlayed == 3)
        {
            return;
        }

        if ((today != DayOfWeek.Wednesday || !_config.EnableWednesdayReminder)
            && (today != DayOfWeek.Sunday || !_config.EnableSundayReminder))
        {
            return;
        }

        if (_config.RemindOnlyUnknownRecipes)
        {
            var tv = new TvWrapper();
            var recipe = tv.GetTodayRecipe();
            if (Game1.player.knowsRecipe(recipe))
            {
                return;
            }
        }

        Game1.drawObjectDialogue(Helper.Translation.Get("queen-of-sauce.reminder"));
    }
}
