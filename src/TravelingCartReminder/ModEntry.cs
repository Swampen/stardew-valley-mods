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

        if (today is DayOfWeek.Friday or DayOfWeek.Sunday)
        {
            Game1.morningQueue.Enqueue(() => Game1.showGlobalMessage(Helper.Translation.Get("traveling-cart.arrived.forest")));
        }

        // Show VisitMountVapius reminder if mod is loaded and it's Tuesday or Saturday
        // https://visitmountvapius.wiki.gg/wiki/The_Market
        if (_isVisitMountVapiusLoaded && today is DayOfWeek.Tuesday or DayOfWeek.Saturday)
        {
            Game1.morningQueue.Enqueue(() => Game1.showGlobalMessage(Helper.Translation.Get("traveling-cart.arrived.vapius")));
        }

        // Show RidgesideVillage reminder if mod is loaded and it's Wednesday
        // https://github.com/Rafseazz/Ridgeside-Village-Mod/blob/main/Ridgeside%20SMAPI%20Component%202.0/RidgesideVillage/TravelingCart.cs#L28
        if (_isRidgesideVillageLoaded && today == DayOfWeek.Wednesday)
        {
            Game1.morningQueue.Enqueue(() => Game1.showGlobalMessage(Helper.Translation.Get("traveling-cart.arrived.ridge")));
        }
    }
}