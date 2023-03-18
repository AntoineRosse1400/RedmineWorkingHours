using Microsoft.Extensions.DependencyInjection;
using RedmineWorkingHours.ConsoleApp.Calculator;
using RedmineWorkingHours.ConsoleApp.Configuration;
using RedmineWorkingHours.ConsoleApp.Utils;

namespace RedmineWorkingHours.ConsoleApp;

internal class HoursMenuManager
{
    #region Members

    private readonly ServiceProvider _serviceProvider;

    #endregion

    #region Constructor

    public HoursMenuManager(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Menu

    public void Run()
    {
        var appConfiguration = _serviceProvider.GetService<AppConfiguration>();
        if (appConfiguration == null)
            throw new NullReferenceException($"{nameof(AppConfiguration)} service not found.");

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = DateTime.Now.AddMonths(-1);
        end = DateTimeUtils.GetLastDateOfMonth(end.Year, end.Month);

        var hoursCalculator = _serviceProvider.GetService<IHoursCalculator>();
        if (hoursCalculator == null)
            throw new NullReferenceException($"{nameof(IHoursCalculator)} service not found.");

        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);
        Console.WriteLine($"Hours balance: {hoursBalance}");
    }

    #endregion
}