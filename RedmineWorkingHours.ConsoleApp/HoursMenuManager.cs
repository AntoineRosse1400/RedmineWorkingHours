using Microsoft.Extensions.DependencyInjection;
using RedmineWorkingHours.ConsoleApp.Calculator;
using RedmineWorkingHours.ConsoleApp.Configuration;
using RedmineWorkingHours.ConsoleApp.Utils;

namespace RedmineWorkingHours.ConsoleApp;

internal class HoursMenuManager
{
    #region Members

    private readonly ServiceProvider _serviceProvider;

    #endregion

    #region Constructor

    public HoursMenuManager(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Menu

    public void Run()
    {
        PrintTitle();
        bool mustExit;
        do
        {
            mustExit = this.PrintMenuAndAskUserEntry();
        } while (!mustExit);
    }

    private void PrintTitle()
    {
        Console.WriteLine("==================== WORKING HOURS BALANCE ====================");
    }

    private bool PrintMenuAndAskUserEntry()
    {
        PrintMenu();
        return UserEntry();
    }

    private void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Select the action to execute:");
        int menuIndex = 0;
        Console.WriteLine($"{++menuIndex}. Hours balance until yesterday");
        Console.WriteLine($"{++menuIndex}. Hours balance until today");
        Console.WriteLine($"{++menuIndex}. Hours balance for specific month");
        Console.WriteLine($"{++menuIndex}. Hours balance until specific date");
        Console.WriteLine($"{++menuIndex}. Hours balance between 2 specific dates");
        Console.WriteLine($"{++menuIndex}. Vacation days balance until now");
        Console.WriteLine($"{++menuIndex}. Vacation days balance until end of year");
        Console.WriteLine("0. Exit");
    }

    private bool UserEntry()
    {
        string? userEntry = Console.ReadLine();
        if (userEntry == null)
            return false;

        if (!int.TryParse(userEntry, out int actionIndex))
            return false;

        return ExecuteMenuAction(actionIndex);
    }

    private bool ExecuteMenuAction(int actionIndex)
    {
        switch (actionIndex)
        {
            case 0:
                return true;
            case 1:
                PrintHoursBalanceUntilYesterday();
                break;
            case 2:
                PrintHoursBalanceUntilToday();
                break;
            case 3:
                PrintHoursBalanceForMonth();
                break;
            case 4:
                PrintHoursBalanceUntilDate();
                break;
            case 5:
                PrintHoursBalanceBetween();
                break;
            case 6:
                PrintVacationDaysBalanceUntilNow();
                break;
            case 7:
                PrintVacationDaysBalanceUntilEndOfYear();
                break;
            default:
                return false;
        }
        return false;
    }

    #region Hours balance

    private void PrintHoursBalanceUntilYesterday()
    {
        var appConfiguration = GetService<AppConfiguration>();

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);

        var hoursCalculator = GetService<IHoursCalculator>();
        double hoursBalance = hoursCalculator.GetHoursBalance(begin, DateTime.Now.AddDays(-1));

        Console.WriteLine($"Hours balance from {begin.Date.ToShortDateString()} " +
                          $"until yesterday: {FormatToTwoDecimals(hoursBalance)} hours");
    }

    private void PrintHoursBalanceUntilToday()
    {
        var appConfiguration = GetService<AppConfiguration>();

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);

        var hoursCalculator = GetService<IHoursCalculator>();
        double hoursBalance = hoursCalculator.GetHoursBalance(begin, DateTime.Now);

        Console.WriteLine($"Hours balance from {begin.Date.ToShortDateString()} " +
                          $"until today: {FormatToTwoDecimals(hoursBalance)} hours");
    }

    private void PrintHoursBalanceForMonth()
    {
        var appConfiguration = GetService<AppConfiguration>();
        int year = GetIntUserEntry("Year", minValue: appConfiguration.HoursCalculatorConfiguration.StartYearIndex, maxValue: DateTime.MaxValue.Year);
        int month = GetIntUserEntry("Month", 1, 12);
        DateTime begin = DateTimeUtils.GetFirstDateOfMonth(year, month);
        DateTime end = DateTimeUtils.GetLastDateOfMonth(year, month);

        var hoursCalculator = GetService<IHoursCalculator>();
        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);

        Console.WriteLine($"Hours balance for {year}.{month}: " +
                          $"{FormatToTwoDecimals(hoursBalance)} hours");
    }

    private void PrintHoursBalanceUntilDate()
    {
        var appConfiguration = GetService<AppConfiguration>();
        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = GetDateTimeEntry("Date until", begin, DateTime.MaxValue);

        var hoursCalculator = GetService<IHoursCalculator>();
        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);

        Console.WriteLine($"Hours balance from {begin.Date.ToShortDateString()} " +
                          $"until {end.Date.ToShortDateString()}: " +
                          $"{FormatToTwoDecimals(hoursBalance)} hours");
    }

    private void PrintHoursBalanceBetween()
    {
        var appConfiguration = GetService<AppConfiguration>();
        DateTime minValue = DateTimeUtils.GetFirstDateOfMonth(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex);
        DateTime begin = GetDateTimeEntry("Date from", minValue, DateTime.MaxValue);
        DateTime end = GetDateTimeEntry("Date until", begin, DateTime.MaxValue);

        var hoursCalculator = GetService<IHoursCalculator>();
        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);

        Console.WriteLine($"Hours balance from {begin.Date.ToShortDateString()} " +
                          $"until {end.Date.ToShortDateString()}: " +
                          $"{FormatToTwoDecimals(hoursBalance)} hours");
    }

    #endregion

    #region Vacation balance

    private void PrintVacationDaysBalanceUntilNow()
    {
        var hoursCalculator = this.GetService<IHoursCalculator>();
        double remainingVacationDays = hoursCalculator.GetRemainingVacationDays(DateTime.Now);
        Console.WriteLine($"Remaining vacation days: {FormatToTwoDecimals(remainingVacationDays)} days");
    }

    private void PrintVacationDaysBalanceUntilEndOfYear()
    {
        var hoursCalculator = this.GetService<IHoursCalculator>();
        DateTime endOfYear = new DateTime(DateTime.Now.Year, 12, 31);
        double remainingVacationDays = hoursCalculator.GetRemainingVacationDays(endOfYear);
        Console.WriteLine($"Remaining vacation days: {FormatToTwoDecimals(remainingVacationDays)} days");
    }

    #endregion

    #endregion

    #region Utils

    private static string FormatToTwoDecimals(double value)
    {
        return string.Format("{0:N2}", value);
    }

    private T GetService<T>()
    {
        var service = _serviceProvider.GetService<T>();
        if (service == null)
            throw new NullReferenceException($"{typeof(T).Name} service not found.");
        return service;
    }

    private int GetIntUserEntry(string valueName, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        bool validEntry = false;
        int userEntryInt = 0;
        do
        {
            Console.Write(valueName + ": ");
            string? userEntry = Console.ReadLine();
            if (userEntry == null)
                continue;

            validEntry = int.TryParse(userEntry, out userEntryInt);
            if (!validEntry)
                Console.WriteLine("Not a valid number");
            else if (userEntryInt < minValue && userEntryInt > maxValue)
            {
                Console.WriteLine($"Value must be between {minValue} && {maxValue}");
                validEntry = false;
            }
        } while (!validEntry);

        return userEntryInt;
    }

    private DateTime GetDateTimeEntry(string valueName, DateTime minValue, DateTime maxValue)
    {
        bool validEntry = false;
        DateTime userEntryDateTime = DateTime.Now;
        do
        {
            Console.Write(valueName + " (dd.mm.yyy): ");
            string? userEntry = Console.ReadLine();
            if (userEntry == null)
                continue;

            validEntry = DateTime.TryParse(userEntry, out userEntryDateTime);
            if (!validEntry)
                Console.WriteLine("Not a valid date");
            else if (userEntryDateTime < minValue || userEntryDateTime > maxValue)
            {
                Console.WriteLine($"Value must be between {minValue.ToShortDateString()} && {maxValue.ToShortDateString()}");
                validEntry = false;
            }
        } while (!validEntry);

        return userEntryDateTime;
    }

    #endregion
}