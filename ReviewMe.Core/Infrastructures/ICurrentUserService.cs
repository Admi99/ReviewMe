using System.Security.Claims;
using System.Security.Principal;

namespace ReviewMe.Core.Infrastructures;

public interface ICurrentUserService
{
    WindowsIdentity? UserIdentity { get; }

    string UserNameWithoutDomain { get; }

    void SetUserFromJwtTokenClaims(ClaimsPrincipal principal);
}