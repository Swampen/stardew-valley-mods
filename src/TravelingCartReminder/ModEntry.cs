using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TravelingCartReminder;

public class ModEntry : Mod
{
    private bool _isVisitMountVapiusLoaded;
    private bool _isRidgesideVillageLoaded;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.DayStarted += DayStarted;
    }

    /// <summary>
    /// Checks if other mods is installed
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="args">Event arguments for an GameLaunched event</param>
    private void OnGameLaunched(object? sender, GameLaunchedEventArgs args)
    {
        _isVisitMountVapiusLoaded = Helper.ModRegistry.IsLoaded("lumisteria.visitmountvapius.code");
        _isRidgesideVillageLoaded = Helper.ModRegistry.IsLoaded("Rafseazz.RidgesideVillage");
    }

    /// <summary>Raised after the days starts.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void DayStarted(object? sender, DayStartedEventArgs e)
    {
        var today = SDate.Now().DayOfWeek;
        DayOfWeek[] reminderDays;
        if (_isRidgesideVillageLoaded)
        {
            reminderDays = new[] { DayOfWeek.Wednesday };
        }
        else if (_isVisitMountVapiusLoaded)
        {
            reminderDays = new[] { DayOfWeek.Tuesday, DayOfWeek.Saturday };
        }
        else
        {
            reminderDays = new[] { DayOfWeek.Friday, DayOfWeek.Sunday };
        }

        if (!reminderDays.Contains(today))
        {
            return;
        }

        Game1.morningQueue.Enqueue(() => Game1.showGlobalMessage(Helper.Translation.Get("traveling-cart.arrived")));
    }
}
