using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewMe.Core.Infrastructures;

namespace ReviewMe.Infrastructure.RazorTemplates;

public class DiConfigRazorTemplates
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRazorTemplateParsingService, RazorTemplateParsingService>();
    }
}