using RedmineWorkingHours.ConsoleApp.Communication;
using RedmineWorkingHours.ConsoleApp.Configuration;
using RedmineWorkingHours.ConsoleApp.Utils;

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
        int workingDays = this.GetWorkingDays(begin, end);
        return workingDays * _configuration.ExpectedDailyHours * _configuration.WorkingPercentage;
    }

    private int GetWorkingDays(DateTime begin, DateTime end)
    {
        int weekDaysCount = DateTimeUtils.GetWeekDays(begin, end);

        int publicHolidaysCount = _configuration.PublicHolidays
            .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
            .Count(d => d >= begin && d <= end);

        return weekDaysCount - publicHolidaysCount;
    }

    public double GetRemainingVacationDays(DateTime end)
    {
        var begin = new DateTime(_configuration.StartYearIndex, _configuration.StartMonthIndex, 1);

        double remainingVacationDays = 0.0;
        for (int year = _configuration.StartYearIndex; year <= end.Year; year++)
        {
            double allowedVacationDays = GetAllowedVacationDaysForYear(year);
            double takenVacationDays = 0.0;
            if (begin.Year == end.Year)
                takenVacationDays += this.GetTakenVacationDays(year, begin.Month, end.Month);
            else if (year == end.Year)
                takenVacationDays += this.GetTakenVacationDays(year, 1, end.Month);
            else if (year == begin.Year)
                takenVacationDays += this.GetTakenVacationDays(year, begin.Month, 12);
            else
                takenVacationDays += this.GetTakenVacationDays(year, 1, 12);

            remainingVacationDays += allowedVacationDays - takenVacationDays;
        }

        return remainingVacationDays;
    }

    private double GetAllowedVacationDaysForYear(int year)
    {
        if (year == _configuration.StartYearIndex)
            return (12.0 - _configuration.StartMonthIndex + 1.0) / 12.0 * _configuration.FullTimeYearlyVacationDays * _configuration.WorkingPercentage;

        if (_configuration.EndYearIndex == null || _configuration.EndMonthIndex == null)
            return _configuration.FullTimeYearlyVacationDays * _configuration.WorkingPercentage;

        if (year == _configuration.EndYearIndex)
            return _configuration.EndMonthIndex.Value * _configuration.FullTimeYearlyVacationDays * _configuration.WorkingPercentage;

        return _configuration.FullTimeYearlyVacationDays * _configuration.WorkingPercentage;
    }

    private double GetTakenVacationDays(int year, int beginMonth, int endMonth)
    {
        DateTime begin = new DateTime(year, beginMonth, 1);
        DateTime end = DateTimeUtils.GetLastDateOfMonth(year, endMonth);
        double takenVacationDays = _hoursReader.GetVacationDaysBetween(begin, end);
        return takenVacationDays;
    }

    #endregion
}