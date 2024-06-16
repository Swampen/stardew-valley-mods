using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace TheQueenOfSauceReminder
{
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
                var recipe = GetTodaysRecipe();
                if (Game1.player.knowsRecipe(recipe))
                {
                    return;
                }
            }

            Game1.drawObjectDialogue("Reminder: The Queen of Sauce is airing on TV today");
        }

        /// <summary>
        /// Gets today's recipe
        /// </summary>
        /// <remarks>Code from protected method <see cref="StardewValley.Objects.TV.getWeeklyRecipe()"/></remarks>
        /// <returns>Today's recipe</returns>
        private static string GetTodaysRecipe()
        {
            // Retrieves all recipes
            var weekToRecipeMap = new Dictionary<int, string>();
            var dictionary = DataLoader.Tv_CookingChannel(Game1.temporaryContent);
            foreach (var key in dictionary.Keys)
            {
                weekToRecipeMap[Convert.ToInt32(key)] = dictionary[key].Split('/')[0];
            }

            var recipeNumber = (int)(Game1.stats.DaysPlayed % 224 / 7);
            recipeNumber = recipeNumber == 0 ? 32 : recipeNumber;

            // Return normal show
            if (SDate.Now().DayOfWeek == DayOfWeek.Sunday)
            {
                return weekToRecipeMap[recipeNumber].Split('/')[0];
            }

            // Get a random unknown recipe
            var team = Game1.player.team;
            if (team.lastDayQueenOfSauceRerunUpdated.Equals(Game1.Date.TotalDays) == false)
            {
                team.lastDayQueenOfSauceRerunUpdated.Set(Game1.Date.TotalDays);
                team.queenOfSauceRerunWeek.Set(GetRerunWeek(weekToRecipeMap));
            }

            recipeNumber = team.queenOfSauceRerunWeek.Value;
            return weekToRecipeMap[recipeNumber].Split('/')[0];
        }

        /// <summary>
        /// Gets a random recipe that has aired before.
        /// </summary>
        /// <param name="weekToRecipeMap">Dictionary with all the recipes</param>
        /// <remarks>Code from protected method in <see cref="StardewValley.Objects.TV.getRerunWeek()"/></remarks>
        /// <returns>Returns a random unknown recipe or a random
        /// known recipe if all currently aired recipes is already known</returns>
        private static int GetRerunWeek(Dictionary<int, string> weekToRecipeMap)
        {
            var maxValue = Math.Min(((int)Game1.stats.DaysPlayed - 3) / 7, 32);
            var recipePool = new List<int>();
            var allFarmers = Game1.getAllFarmers().ToList();
            for (var key = 1; key <= maxValue; key++)
            {
                if (allFarmers.Any(farmer => !farmer.cookingRecipes.ContainsKey(weekToRecipeMap[key])))
                {
                    recipePool.Add(key);
                }
            }

            var random = Utility.CreateDaySaveRandom();
            return recipePool.Count != 0
                ? recipePool[random.Next(recipePool.Count)]
                : Math.Max(1, 1 + random.Next(maxValue));
        }
    }
}