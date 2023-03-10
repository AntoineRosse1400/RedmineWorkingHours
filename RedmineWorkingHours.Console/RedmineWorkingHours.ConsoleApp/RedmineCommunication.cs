using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Collections.Specialized;
using System.Globalization;

namespace RedmineWorkingHours.ConsoleApp;

internal class RedmineCommunication
{
    #region Constants

    private const string RedmineDateFormat = "yyyy-MM-dd";

    #endregion

    #region Members

    private RedmineManager _manager;

    #endregion

    #region Constructor

    internal RedmineCommunication(string serverUrl, string apiKey, int targetUserId)
    {
        _manager = new RedmineManager(serverUrl, apiKey);
        DateTime beginWeekDay = GetFirstDateOfWeekISO8601(2023, 10);
        DateTime endWeekDay = beginWeekDay.AddDays(5);
        var parameters = new NameValueCollection
        {
            { RedmineKeys.USER_ID, targetUserId.ToString() },
            { RedmineKeys.SPENT_ON, $"><{beginWeekDay.ToString(RedmineDateFormat)}|{endWeekDay.ToString(RedmineDateFormat)}" }
        };
        IEnumerable<TimeEntry> weeklyTimeEntries = _manager.GetObjects<TimeEntry>(parameters);
    }

    public static DateTime GetFirstDateOfWeekISO8601(int year, int weekOfYear)
    {
        DateTime jan1 = new DateTime(year, 1, 1);
        int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        // Use first Thursday in January to get first week of the year as
        // it will never be in Week 52/53
        DateTime firstThursday = jan1.AddDays(daysOffset);
        var cal = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        var weekNum = weekOfYear;
        // As we're adding days to a date in Week 1,
        // we need to subtract 1 in order to get the right date for week #1
        if (firstWeek == 1)
        {
            weekNum -= 1;
        }

        // Using the first Thursday as starting week ensures that we are starting in the right year
        // then we add number of weeks multiplied with days
        var result = firstThursday.AddDays(weekNum * 7);

        // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
        return result.AddDays(-3);
    }

    #endregion
}