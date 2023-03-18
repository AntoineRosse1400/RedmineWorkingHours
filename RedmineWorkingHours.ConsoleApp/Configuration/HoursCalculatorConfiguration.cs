namespace RedmineWorkingHours.ConsoleApp.Configuration;

public class HoursCalculatorConfiguration
{
    public int StartYearIndex { get; set; }

    public int StartMonthIndex { get; set; }

    public double WorkingPercentage { get; set; }

    public List<YearlyWorkingHours> YearlyWorkingDays { get; set; } = new();
}