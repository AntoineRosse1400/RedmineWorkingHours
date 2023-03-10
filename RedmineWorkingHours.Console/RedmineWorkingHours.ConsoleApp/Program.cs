using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Collections.Specialized;

namespace RedmineWorkingHours.ConsoleApp;

static class Program
{
    #region Constants

    private const string ServerUrl = "https://redmine.adonite.com/";
    private const string ApiKey = "ed8946d0d93fe5b8acba4e47801fa7e469fcf38b";
    private const int TargetUserId = 57;

    #endregion

    #region Members

    private static RedmineCommunication _redmineCommunication;

    #endregion

    private static void Main(string[] args)
    {
        _redmineCommunication = new RedmineCommunication(ServerUrl, ApiKey, TargetUserId);

        double currentWeekHours = _redmineCommunication.GetWeekHours(2023, 10);
        Console.WriteLine($"Current week hours: {currentWeekHours}");
    }
}