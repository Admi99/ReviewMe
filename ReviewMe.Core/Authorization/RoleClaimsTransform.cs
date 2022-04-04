using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ReviewMe.Core.Settings;
using System.Security.Claims;
using System.Security.Principal;

#pragma warning disable CA1416 // Validate platform compatibility

namespace ReviewMe.Core.Authorization;

internal sealed class RoleClaimsTransform : IClaimsTransformation
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IOptionsMonitor<ApplicationSettings> _applicationSettingsMonitoredOptions;

    public RoleClaimsTransform(ICurrentUserService currentUserService, IOptionsMonitor<ApplicationSettings> applicationSettingsMonitoredOptions)
    {
        _currentUserService = currentUserService;
        _applicationSettingsMonitoredOptions = applicationSettingsMonitoredOptions;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        return await Task.FromResult(GetClaimsPrincipal(principal));
    }

    private ClaimsPrincipal GetClaimsPrincipal(ClaimsPrincipal principal)
    {
        var isForTesting = _applicationSettingsMonitoredOptions.CurrentValue.IsStartedForTesting;
        var roleAccountMappingSettings = _applicationSettingsMonitoredOptions.CurrentValue.RoleTypeWithAccountSettings;

        if (isForTesting)
        {
            _currentUserService.UserNameWithoutDomain = principal!.Claims.SingleOrDefault(c => c.Type == "login")!.Value;
            var roleTypes = new List<AuthRoleTypes>(32);
            foreach (var roleTypeWithAccountSetting in roleAccountMappingSettings!)
            {
                AddRoleTypes(roleTypeWithAccountSetting, roleTypes);
            }

            var identity = new ClaimsIdentity();

            var claims = roleTypes
              .Distinct()
              .Select(rt => new Claim(AuthorizationPolicies.RoleClaimType, rt.ToString()));

            identity.AddClaims(claims);

            principal.AddIdentity(identity);
            return principal;
        }
        else
        {
            _currentUserService.SetUserFromJwtTokenClaims(principal);

            var identity = _currentUserService.UserIdentity;

            if (identity?.Groups == null)
            {
                return principal;
            }

            var translatedGroups = identity.Groups
            .Select(x => x.Translate(typeof(NTAccount)).ToString())
            .ToArray();

            var roleTypes = new List<AuthRoleTypes>(32);

            foreach (var roleTypeWithAccountSetting in roleAccountMappingSettings!)
            {
                if (IsUserInTheAccountSettings(identity.Name, roleTypeWithAccountSetting.Accounts!, translatedGroups))
                {
                    AddRoleTypes(roleTypeWithAccountSetting, roleTypes);
                }
            }

            var claims = roleTypes
                .Distinct()
                .Select(rt => new Claim(AuthorizationPolicies.RoleClaimType, rt.ToString()));

            identity.AddClaims(claims);

            return new ClaimsPrincipal(identity);
        }
    }

    private static void AddRoleTypes(RoleTypeWithAccountSetting roleTypeWithAccountSetting, ICollection<AuthRoleTypes> roleTypes)
    {
        switch (roleTypeWithAccountSetting.RoleType)
        {
            case AuthRoleTypes.Employee:
                roleTypes.Add(AuthRoleTypes.Employee);
                break;

            case AuthRoleTypes.SuperAdmin:
                roleTypes.Add(AuthRoleTypes.Employee);
                roleTypes.Add(AuthRoleTypes.SuperAdmin);
                break;

            default:
                throw new ApplicationException($"Unsupported RoleType='{roleTypeWithAccountSetting.RoleType}'!");
        }
    }

    private static bool IsUserInTheAccountSettings(string userName, string[] accountSettings, string[] translatedGroups)
    {
        return accountSettings
            .Any(accountSetting =>
                accountSetting.Equals(userName, StringComparison.InvariantCultureIgnoreCase) ||
                translatedGroups.Any(tg => tg.Equals(accountSetting, StringComparison.InvariantCultureIgnoreCase))
            );

    }
}