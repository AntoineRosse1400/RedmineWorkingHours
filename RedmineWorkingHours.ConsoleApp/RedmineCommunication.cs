using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Collections.Specialized;

namespace RedmineWorkingHours.ConsoleApp;

internal class RedmineCommunication
{
    #region Constants

    private const string RedmineDateFormat = "yyyy-MM-dd";

    #endregion

    #region Members

    private readonly RedmineManager _manager;
    private readonly int _targetUserId;

    #endregion

    #region Constructor

    internal RedmineCommunication(string serverUrl, string apiKey, int targetUserId)
    {
        _manager = new RedmineManager(serverUrl, apiKey);
        _targetUserId = targetUserId;
    }

    #endregion

    #region Get data from Redmine

    internal double GetWeekHours(int year, int weekIndex)
    {
        DateTime beginWeekDay = DateTimeUtils.GetFirstDateOfWeekISO8601(year, weekIndex);
        DateTime endWeekDay = beginWeekDay.AddDays(5);
        var parameters = new NameValueCollection
        {
            { RedmineKeys.USER_ID, _targetUserId.ToString() },
            { RedmineKeys.SPENT_ON, $"><{beginWeekDay.ToString(RedmineDateFormat)}|{endWeekDay.ToString(RedmineDateFormat)}" }
        };
        IEnumerable<TimeEntry> weeklyTimeEntries = _manager.GetObjects<TimeEntry>(parameters);
        return (double)weeklyTimeEntries.Select(w => w.Hours).Sum();
    }

    internal double GetMonthHours(int year, int month)
    {
        if(year > DateTime.Now.Year)
            throw new ArgumentOutOfRangeException(nameof(year), "Year must be smaller or equal to current year");

        if (month is < 1 or > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12");

        DateTime beginMonthDay = DateTimeUtils.GetFirstDateOfMonth(year, month);
        DateTime endMonthDay = DateTimeUtils.GetLastDateOfMonth(year, month);
        var parameters = new NameValueCollection
        {
            { RedmineKeys.USER_ID, _targetUserId.ToString() },
            { RedmineKeys.SPENT_ON, $"><{beginMonthDay.ToString(RedmineDateFormat)}|{endMonthDay.ToString(RedmineDateFormat)}" }
        };
        IEnumerable<TimeEntry> monthlyTimeEntries = _manager.GetObjects<TimeEntry>(parameters);
        return (double)monthlyTimeEntries.Select(w => w.Hours).Sum();
    }

    #endregion
}