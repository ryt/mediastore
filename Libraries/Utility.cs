using System;

namespace mediastore.Libraries
{
    public class Utility
    {
        public static string RelativeDate(DateTime date)
        {
            var result = "";
            var timeSpan = DateTime.UtcNow.Subtract(date);
            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = timeSpan.Seconds > 1 ? string.Format("{0} secs", timeSpan.Seconds) : "1 sec";
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ? String.Format("{0} mins", timeSpan.Minutes) : "1 min";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ? String.Format("{0} hrs", timeSpan.Hours) : "1 hr";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ? String.Format("{0} days", timeSpan.Days) : "1 day";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ? String.Format("{0} mos", timeSpan.Days / 30) : "1 mo";
            }
            else
            {
                result = timeSpan.Days > 365 ? String.Format("{0} yrs", timeSpan.Days / 365) : "1 yr";
            }
            return result;
        }
    }
}