using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedmineWorkingHours.ConsoleApp;

static class Program
{
    #region Constants

    private const string AppSettingsFileName = "appsettings.json";

    #endregion

    #region Members

    private static IHoursCalculator _hoursCalculator;

    #endregion

    #region Main

    private static void Main(string[] args)
    {
        ServiceProvider serviceProvider = ConfigureServices();
        _hoursCalculator = new RedmineHoursCalculator(serviceProvider.GetService<AppConfiguration>());

        double hoursBalance = _hoursCalculator.GetHoursBalance(new DateTime(2023, 2, 1), new DateTime(2023, 2, 28));
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