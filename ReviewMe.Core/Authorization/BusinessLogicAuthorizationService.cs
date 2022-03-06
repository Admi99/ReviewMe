using Microsoft.AspNetCore.Authorization;
using System.Security.Authentication;
using System.Security.Principal;

#pragma warning disable CA1416 // Validate platform compatibility

namespace ReviewMe.Core.Authorization;

internal sealed class BusinessLogicAuthorizationService : IBusinessLogicAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUserService _currentUserService;

    public BusinessLogicAuthorizationService(IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
    }

    public async Task AuthorizeAsync(string policyName)
    {
        await AuthorizeUserWitOptionalResource(policyName);
    }

    public async Task AuthorizeAsync<T>(string policyName, T? requestToAuthorize)
    {
        await AuthorizeUserWitOptionalResource(policyName, requestToAuthorize);
    }

    private async Task AuthorizeUserWitOptionalResource(string policyName, object? resource = null)
    {
        if (_currentUserService.UserIdentity != null)
        {
            var user = new WindowsPrincipal(_currentUserService.UserIdentity);

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, policyName);

            if (authorizationResult.Succeeded == false)
            {
                throw new AuthenticationException($"The user '{user.Identity.Name}' is not authorized for the operation. The failing policy is '{policyName}'.");
            }
        }
    }
}