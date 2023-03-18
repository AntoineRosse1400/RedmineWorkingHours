namespace RedmineWorkingHours.ConsoleApp.Configuration;

public class YearlyWorkingHours
{
    public int Year { get; set; }

    public List<int> MonthlyWorkingHours { get; set; } = new();
}