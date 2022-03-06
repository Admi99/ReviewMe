using Destructurama;
using Serilog;

namespace ReviewMe.API.Extensions;

internal static class BuilderExtensions
{
    internal static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Destructure.UsingAttributes()
            .CreateLogger(); 

        builder.Host.UseSerilog((context, configuration) 
            => configuration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));
        return builder; 
    }
}