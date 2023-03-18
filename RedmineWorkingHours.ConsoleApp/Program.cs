using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedmineWorkingHours.ConsoleApp.Calculator;
using RedmineWorkingHours.ConsoleApp.Communication;
using RedmineWorkingHours.ConsoleApp.Communication.Redmine;
using RedmineWorkingHours.ConsoleApp.Configuration;
using RedmineWorkingHours.ConsoleApp.Utils;
using System;

namespace RedmineWorkingHours.ConsoleApp;

static class Program
{
    #region Constants

    private const string AppSettingsFileName = "D:/Adonite/Data/RedmineWorkingHours/appsettings.json";

    #endregion

    #region Members

    private static IServiceProvider _serviceProvider;

    #endregion

    #region Main

    private static void Main(string[] args)
    {
        ConfigureServices();

        var appConfiguration = _serviceProvider.GetService<AppConfiguration>();
        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = DateTime.Now.AddMonths(-1);
        end = DateTimeUtils.GetLastDateOfMonth(end.Year, end.Month);

        double hoursBalance = _serviceProvider.GetService<IHoursCalculator>().GetHoursBalance(begin, end);
        Console.WriteLine($"Hours balance: {hoursBalance}");
    }

    #endregion

    #region Services

    private static void ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
            .AddJsonFile(AppSettingsFileName, optional: true, reloadOnChange: true)
            .Build();
        services.AddSingleton(_ => GetAppConfiguration(configuration));
        services.AddSingleton<IHoursReader, RedmineCommunication>();
        services.AddSingleton<IHoursCalculator, HoursCalculator>();
        _serviceProvider = services.BuildServiceProvider();
    }

    private static AppConfiguration GetAppConfiguration(IConfiguration configuration)
    {
        AppConfiguration appConfiguration = new();
        configuration.Bind(appConfiguration);
        return appConfiguration;
    }

    #endregion
}