using RedmineWorkingHours.ConsoleApp.Communication;
using RedmineWorkingHours.ConsoleApp.Communication.Redmine;
using RedmineWorkingHours.ConsoleApp.Configuration;

namespace RedmineWorkingHours.ConsoleApp.Calculator;

internal class HoursCalculator : IHoursCalculator
{
    #region Members

    private readonly IHoursReader _hoursReader;
    private readonly HoursCalculatorConfiguration _configuration;

    #endregion

    #region Constructor

    public HoursCalculator(AppConfiguration configuration, IHoursReader hoursReader)
    {
        _configuration = configuration.HoursCalculatorConfiguration;
        _hoursReader = hoursReader;
    }

    #endregion

    #region Hours calculation

    public double GetHoursBalance(DateTime begin, DateTime end)
    {
        double expectedHours = GetExpectedHours(begin, end);
        double workedHours = _hoursReader.GetWorkedHoursBetween(begin, end);
        double vacationHours = _hoursReader.GetVacationDaysBetween(begin, end) * 8.0 * _configuration.WorkingPercentage;
        return workedHours - expectedHours + vacationHours;
    }

    private double GetExpectedHours(DateTime begin, DateTime end)
    {
        double expectedHours = 0.0;
        for (int year = begin.Year; year <= end.Year; year++)
        {
            if (begin.Year == end.Year)
                expectedHours += GetExpectedHours(year, begin.Month, end.Month);
            else if (year == end.Year)
                expectedHours += GetExpectedHours(year, 1, end.Month);
            else if (year == begin.Year)
                expectedHours += GetExpectedHours(year, begin.Month, 12);
            else
                expectedHours += GetExpectedHours(year, 1, 12);
        }
        return expectedHours * _configuration.WorkingPercentage;
    }

    private double GetExpectedHours(int year, int beginMonth, int endMonth)
    {
        double expectedHours = 0.0;
        YearlyWorkingHours? beginYearWorkingHours = _configuration
            .YearlyWorkingHours
            .FirstOrDefault(y => y.Year == year);
        if (beginYearWorkingHours == null)
            throw new InvalidOperationException($"No data found for year {year} in configuration. Please check the configuration file.");
        for (int month = beginMonth; month <= endMonth; month++)
            expectedHours += beginYearWorkingHours.MonthlyWorkingHours[month - 1];
        return expectedHours;
    }

    public double GetRemainingVacationHours(DateTime end)
    {
        throw new NotImplementedException();
    }

    #endregion
}