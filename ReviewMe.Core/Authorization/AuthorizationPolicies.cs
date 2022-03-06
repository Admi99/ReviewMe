using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ReviewMe.Core.Authorization;

public static class AuthorizationPolicies
{
    public static readonly string RoleClaimType = $"http://{typeof(RoleClaimsTransform).FullName?.Replace('.', '/')}/role";

    public const string Employee = "Employee";
    public const string SuperAdmin = "SuperAdmin";

    public static void AddReviewmePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Employee, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireClaim(RoleClaimType, AuthRoleTypes.Employee.ToString());
        });

        options.AddPolicy(SuperAdmin, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireClaim(RoleClaimType, AuthRoleTypes.SuperAdmin.ToString());
        });
    }
}