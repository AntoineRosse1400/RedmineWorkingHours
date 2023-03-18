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
            mustExit = ExecuteOneMenuAction();
        } while (!mustExit);
    }

    private void PrintTitle()
    {
        Console.WriteLine("==================== WORKING HOURS BALANCE ====================");
    }

    private bool ExecuteOneMenuAction()
    {
        PrintMenu();
        return UserEntry();
    }

    private void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Select the action to execute:");
        Console.WriteLine("1. Hours balance until last month");
        Console.WriteLine("2. Vacation days balance until now");
        Console.WriteLine("3. Vacation days balance until end of year");
        Console.WriteLine("0. Exit");
    }

    private bool UserEntry()
    {
        string? userInput = Console.ReadLine();
        if (userInput == null)
            return false;

        if (!int.TryParse(userInput, out int actionIndex))
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
                this.PrintHoursBalance();
                break;
            case 2:
                this.PrintVacationDaysBalanceUntilNow();
                break;
            case 3:
                this.PrintVacationDaysBalanceUntilEndOfYear();
                break;
            default:
                return false;
        }
        return false;
    }

    private void PrintHoursBalance()
    {
        var appConfiguration = this.GetService<AppConfiguration>();

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = DateTime.Now.AddMonths(-1);
        end = DateTimeUtils.GetLastDateOfMonth(end.Year, end.Month);

        var hoursCalculator = this.GetService<IHoursCalculator>();

        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);
        Console.WriteLine($"Hours balance: {FormatToTwoDecimals(hoursBalance)} hours");
    }

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
}