namespace ReviewMe.Core.Settings;

public sealed class ApplicationSettings
{
    public RoleTypeWithAccountSetting[]? RoleTypeWithAccountSettings { get; set; }
    public bool IsStartedForTesting { get; set; }
    public bool UseTestEmailAddresses { get; set; }
    public List<string> TestEmailAddresses { get; set; } = new();
}