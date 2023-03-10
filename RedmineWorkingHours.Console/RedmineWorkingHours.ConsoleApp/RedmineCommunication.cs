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

    #region MyRegion

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

    #endregion
}