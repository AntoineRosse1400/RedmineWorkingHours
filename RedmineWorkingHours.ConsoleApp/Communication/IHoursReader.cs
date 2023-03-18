namespace RedmineWorkingHours.ConsoleApp.Communication;

public interface IHoursReader
{
    double GetWorkedHoursBetween(DateTime begin, DateTime end);

    double GetVacationDaysBetween(DateTime begin, DateTime end);
}