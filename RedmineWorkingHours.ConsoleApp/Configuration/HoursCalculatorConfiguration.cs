namespace RedmineWorkingHours.ConsoleApp.Configuration;

public class HoursCalculatorConfiguration
{
    public int StartYearIndex { get; set; }

    public int StartMonthIndex { get; set; }

    public int? EndYearIndex { get; set; }

    public int? EndMonthIndex { get; set; }

    public double ExpectedDailyHours { get; set; }

    public double WorkingPercentage { get; set; }

    public int FullTimeYearlyVacationDays { get; set; }

    public List<DateTime> PublicHolidays { get; set; } = new();
}