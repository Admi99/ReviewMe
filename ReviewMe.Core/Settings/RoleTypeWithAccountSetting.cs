using ReviewMe.Core.Authorization;

namespace ReviewMe.Core.Settings;

public sealed class RoleTypeWithAccountSetting
{
    public AuthRoleTypes RoleType { get; set; }
    public string[]? Accounts { get; set; }
}