namespace RedmineWorkingHours.ConsoleApp;

internal class RedmineHoursCalculator : IHoursCalculator
{
    #region Members

    private readonly RedmineCommunication _redmineCommunication;
    private readonly AppConfiguration _configuration;

    #endregion

    #region Constructor

    public RedmineHoursCalculator(AppConfiguration configuration)
    {
        _configuration = configuration;
        _redmineCommunication = new RedmineCommunication(configuration.RedmineConfiguration.ServerUrl, configuration.RedmineConfiguration.ApiKey, configuration.RedmineConfiguration.TargetUserId);
    }

    #endregion

    #region Hours calculation

    public double GetHoursBalance(DateTime begin, DateTime end)
    {

        return _redmineCommunication.GetHoursBetween(begin, end);
    }

    public double GetRemainingVacationHours(DateTime end)
    {
        throw new NotImplementedException();
    }

    #endregion
}