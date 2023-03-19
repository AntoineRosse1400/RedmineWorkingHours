namespace RedmineWorkingHours.ConsoleApp.Configuration;

public class RedmineConfiguration
{
    public string ServerUrl { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public int TargetUserId { get; set; }
}