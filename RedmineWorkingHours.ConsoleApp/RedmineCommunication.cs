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

    internal double GetHoursBetween(DateTime begin, DateTime end)
    {
        var parameters = new NameValueCollection
        {
            { RedmineKeys.USER_ID, _targetUserId.ToString() },
            { RedmineKeys.SPENT_ON, $"><{begin.ToString(RedmineDateFormat)}|{end.ToString(RedmineDateFormat)}" }
        };
        IEnumerable<TimeEntry>? timeEntries = _manager.GetObjects<TimeEntry>(parameters);
        if (timeEntries == null)
            return 0.0;
        return (double)timeEntries.Select(w => w.Hours).Sum();
    }

    #endregion
}