using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TheQueenOfSauceReminder
{
    public class ModEntry : Mod
    {
        /*********
         ** Properties
         *********/
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig? _config;

        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            _config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.DayStarted += DayStarted;
        }

        /*********
         ** Private methods
         *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void DayStarted(object? sender, DayStartedEventArgs e)
        {
            _config ??= new ModConfig();
            var today = SDate.Now().DayOfWeek;
            if ((today == DayOfWeek.Wednesday && _config.EnableWednesdayReminder)
                || (today == DayOfWeek.Sunday && _config.EnableSundayReminder))
            {
                Game1.drawObjectDialogue("Reminder: The Queen of Sauce is airing on TV today");
            }
        }
    }
}