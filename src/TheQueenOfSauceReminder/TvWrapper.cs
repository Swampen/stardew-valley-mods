using System.Text.RegularExpressions;
using StardewValley.Objects;

namespace TheQueenOfSauceReminder;

public class TvWrapper : TV
{
    /// <summary>
    /// Wrapper method to call protected method <see cref="StardewValley.Objects.TV.getWeeklyRecipe()"/>
    /// </summary>
    /// <returns>Today's recipe</returns>
    public string GetTodayRecipe()
    {
        var todayText = base.getWeeklyRecipe();
        var regex = new Regex("'([\\w ]+)'");
        var match = regex.Match(todayText[1]);
        return match.Groups[1].Value;
    }
}