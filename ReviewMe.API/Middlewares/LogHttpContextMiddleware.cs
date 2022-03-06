using ReviewMe.Core.Infrastructures;
using Serilog.Context;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ReviewMe.API.Middlewares;

public class LogHttpContextMiddleware
{
    private readonly RequestDelegate _next;

    public LogHttpContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext? context, ICurrentUserService currentUserService)
    {
        LogContext.PushProperty("Username", currentUserService.UserIdentity?.Name ?? "[N/A]");
        LogContext.PushProperty("Host", context?.Request.Headers["Origin"] ?? "[N/A]");
        LogContext.PushProperty("UserAgent", context?.Request.Headers["User-Agent"] ?? "[N/A]");

        var claims = currentUserService.UserIdentity?.Claims.Where(c => c.Type == AuthorizationPolicies.RoleClaimType).Select(c => c.Value);

        if (claims != null)
        {
            LogContext.PushProperty("UserRoles", string.Join(';', claims));
        }

        if (context != null)
            return _next(context);

        throw new ApplicationException("HttpContext is null");
    }
}