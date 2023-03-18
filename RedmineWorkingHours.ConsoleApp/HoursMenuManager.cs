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
        var appConfiguration = _serviceProvider.GetService<AppConfiguration>();
        if (appConfiguration == null)
            throw new NullReferenceException($"{nameof(AppConfiguration)} service not found.");

        DateTime begin = new DateTime(appConfiguration.HoursCalculatorConfiguration.StartYearIndex, appConfiguration.HoursCalculatorConfiguration.StartMonthIndex, 1);
        DateTime end = DateTime.Now.AddMonths(-1);
        end = DateTimeUtils.GetLastDateOfMonth(end.Year, end.Month);

        var hoursCalculator = _serviceProvider.GetService<IHoursCalculator>();
        if (hoursCalculator == null)
            throw new NullReferenceException($"{nameof(IHoursCalculator)} service not found.");

        double hoursBalance = hoursCalculator.GetHoursBalance(begin, end);
        Console.WriteLine($"Hours balance: {hoursBalance}");
    }

    private void PrintVacationDaysBalanceUntilNow()
    {
        Console.WriteLine("Working on it...");
    }

    private void PrintVacationDaysBalanceUntilEndOfYear()
    {
        Console.WriteLine("Working on it...");
    }

    #endregion
}