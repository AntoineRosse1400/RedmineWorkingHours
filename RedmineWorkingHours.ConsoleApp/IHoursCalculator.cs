namespace RedmineWorkingHours.ConsoleApp;

public interface IHoursCalculator
{
    /// <summary>
    /// Gets the hours balance between <paramref name="begin"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="begin">The begin date.</param>
    /// <param name="end">The end date.</param>
    /// <returns></returns>
    double GetHoursBalance(DateTime begin, DateTime end);

    /// <summary>
    /// Gets the remaining vacation days after <paramref name="end"/>.
    /// </summary>
    /// <param name="end">The date until which to calculate the balance.</param>
    /// <returns></returns>
    double GetRemainingVacationHours(DateTime end);
}