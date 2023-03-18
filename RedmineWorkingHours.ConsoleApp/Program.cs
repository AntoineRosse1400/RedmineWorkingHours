using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedmineWorkingHours.ConsoleApp.Calculator;
using RedmineWorkingHours.ConsoleApp.Configuration;
using RedmineWorkingHours.ConsoleApp.Utils;

namespace RedmineWorkingHours.ConsoleApp;

static class Program
{
    #region Constants

    private const string AppSettingsFileName = "D:/Adonite/Data/RedmineWorkingHours/appsettings.json";

    #endregion

    #region Members

    private static IHoursCalculator _hoursCalculator;

    #endregion

    #region Main

    private static void Main(string[] args)
    {
        ServiceProvider serviceProvider = ConfigureServices();
        var appConfiguration = serviceProvider.GetService<AppConfiguration>();
        _hoursCalculator = new RedmineHoursCalculator(appConfiguration);

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = DateTime.Now.AddMonths(-1);
        end = DateTimeUtils.GetLastDateOfMonth(end.Year, end.Month);

        double hoursBalance = _hoursCalculator.GetHoursBalance(begin, end);
        Console.WriteLine($"Hours balance: {hoursBalance}");
    }

    #endregion

    #region Services

    private static ServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
            .AddJsonFile(AppSettingsFileName, optional: true, reloadOnChange: true)
            .Build();
        services.AddSingleton(_ => GetAppConfiguration(configuration));
        return services.BuildServiceProvider();
    }

    private static AppConfiguration GetAppConfiguration(IConfiguration configuration)
    {
        AppConfiguration appConfiguration = new();
        configuration.Bind(appConfiguration);
        return appConfiguration;
    }

    #endregion
}