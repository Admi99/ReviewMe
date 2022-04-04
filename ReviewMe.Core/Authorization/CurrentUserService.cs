using Microsoft.Extensions.Options;
using ReviewMe.Core.Settings;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

#pragma warning disable CA1416 // Validate platform compatibility

namespace ReviewMe.Core.Authorization;

internal sealed class CurrentUserService : ICurrentUserService
{
    private string? _usernameWithoutDomain;
    private readonly IOptionsMonitor<ApplicationSettings> _applicationSettingsMonitoredOptions;

    public CurrentUserService(IOptionsMonitor<ApplicationSettings> applicationSettingsMonitoredOptions)
    {
        _applicationSettingsMonitoredOptions = applicationSettingsMonitoredOptions;
        UserNameWithoutDomain = _usernameWithoutDomain ??= GetUserNameWithoutDomain();
    }

    public WindowsIdentity? UserIdentity { get; private set; }
    public string UserNameWithoutDomain { get; set; }

    public void SetUserFromJwtTokenClaims(ClaimsPrincipal principal)
    {
        var login = principal.Claims.SingleOrDefault(c => c.Type == "login")?.Value;

        if (string.IsNullOrEmpty(login))
        {
            throw new AuthenticationException("Login is not provided!");
        }

        try
        {
            UserIdentity = new WindowsIdentity(login);
        }
        catch (Exception ex)
        {
            throw new AuthenticationException($"Unauthorized access for username='{login}'!", ex);
        }
    }

    private string GetUserNameWithoutDomain()
    {
        if(_applicationSettingsMonitoredOptions.CurrentValue.IsStartedForTesting)
        {
            return _applicationSettingsMonitoredOptions.CurrentValue.TestingDomainName;
        }

        if (UserIdentity == null)
        {
            throw new Exception("UserIdentity is null");
        }

        var splitName = UserIdentity.Name.Split("\\");
        if (splitName.Length == 2)
        {
            return splitName[1].Trim();
        }

        splitName = UserIdentity.Name.Split("@");
        if (splitName.Length == 2)
        {
            return splitName[0].Trim();
        }

        return UserIdentity.Name;

    }
}