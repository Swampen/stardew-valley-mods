using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TheQueenOfSauceReminder
{
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += this.DayStarted;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void DayStarted(object sender, DayStartedEventArgs e)
        {
            SDate sdate1 = SDate.Now();
            SDate sdate2 = new SDate(3, "spring");
            SDate sdate3 = new SDate(7, "spring");
            if (sdate1.DayOfWeek != sdate2.DayOfWeek && sdate1.DayOfWeek != sdate3.DayOfWeek)
                return;
            Game1.drawObjectDialogue("Reminder: The Queen of Sauce is airing on TV today");
        }
    }
}
