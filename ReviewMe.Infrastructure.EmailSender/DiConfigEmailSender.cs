using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewMe.Core.Infrastructures;
using ReviewMe.Infrastructure.EmailSender.Settings;

namespace ReviewMe.Infrastructure.EmailSender;

public static class DiConfigEmailSender
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");

        services.Configure<EmailSettings>(emailSettings);

        services.AddSingleton<IEmailSender, Services.EmailSender>();
    }

}