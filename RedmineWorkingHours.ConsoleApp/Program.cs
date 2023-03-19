using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedmineWorkingHours.ConsoleApp.Calculator;
using RedmineWorkingHours.ConsoleApp.Communication;
using RedmineWorkingHours.ConsoleApp.Communication.Redmine;
using RedmineWorkingHours.ConsoleApp.Configuration;

namespace RedmineWorkingHours.ConsoleApp;

static class Program
{
    #region Constants

    private const string AppSettingsFileName = "D:/Adonite/Data/RedmineWorkingHours/appsettings.json";

    #endregion

    #region Members

    private static ServiceProvider? _serviceProvider;

    #endregion

    #region Main

    private static void Main()
    {
        ConfigureServices();
        HoursMenuManager menuManager = new HoursMenuManager(_serviceProvider!);
        menuManager.Run();
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