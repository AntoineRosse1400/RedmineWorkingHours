namespace RedmineWorkingHours.ConsoleApp.Configuration;

internal class AppConfiguration
{
    public RedmineConfiguration RedmineConfiguration { get; } = new();
    public HoursCalculatorConfiguration HoursCalculatorConfiguration { get; } = new();
}