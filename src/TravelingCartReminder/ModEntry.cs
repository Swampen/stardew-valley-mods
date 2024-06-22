using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TravelingCartReminder;

public class ModEntry : Mod
{
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += DayStarted;
    }

    /// <summary>Raised after the days starts.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void DayStarted(object? sender, DayStartedEventArgs e)
    {
        var today = SDate.Now().DayOfWeek;
        if (today is not DayOfWeek.Friday and not DayOfWeek.Sunday)
        {
            return;
        }

        Game1.showGlobalMessage("The traveling cart has arrived");
    }
}