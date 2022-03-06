using Microsoft.EntityFrameworkCore;
using ReviewMe.Infrastructure.EmailSender;
using ReviewMe.Infrastructure.RazorTemplates;
using Serilog;
using System.Diagnostics;
using System.Text;
using ReviewMe.API.Extensions;


var builder = WebApplication.CreateBuilder(args);

ConfigureAppConfiguration(builder.Configuration, args);

builder.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(build => build.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.CustomSchemaIds(x => x.FullName);
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ReviewMe",
        Version = "v1"
    });

    swaggerGenOptions.AddSecurityDefinition("JWT Token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        Description = "Copy 'bearer: '",
        In = ParameterLocation.Header
    });
    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT Token"
                }
            },
            Array.Empty<string>()
        }
    });
});

var authenticationSettingsSection = builder.Configuration.GetSection("AuthenticationSettings");
builder.Services.Configure<AuthenticationSettings>(authenticationSettingsSection);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtBearerOptions =>
    {
        var authenticationSettings = authenticationSettingsSection.Get<AuthenticationSettings>();

        jwtBearerOptions.SaveToken = true;
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.SecretKey ?? string.Empty)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateActor = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.FromSeconds(authenticationSettings.JwtClockSkewInSeconds)
        };

        jwtBearerOptions.IncludeErrorDetails = true;
        jwtBearerOptions.Events = GetJwtDebugEvents();
    });

builder.Services.AddControllers();

DiConfigCore.ConfigureServices(builder.Services, builder.Configuration);
DiConfigRabbitMqConsumer.ConfigureServices(builder.Services, builder.Configuration);
DiConfigDbStorage.ConfigureServices(builder.Services, builder.Configuration);
DiConfigEmailSender.ConfigureServices(builder.Services, builder.Configuration);
DiConfigRazorTemplates.ConfigureServices(builder.Services, builder.Configuration);

//Builds the Web applications
var app = builder.Build();


app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "ReviewMe v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseMiddleware<LogHttpContextMiddleware>();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthorization();

app.UseCors();

app.UseSerilogRequestLogging();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ReviewMeDbContext>();


if (dbContext == null)
{
    throw new NullReferenceException(message: "DbContext is not initialized in DI in Program.cs");
}

dbContext.Database.Migrate();


app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();


static JwtBearerEvents GetJwtDebugEvents()
{
    return new JwtBearerEvents
    {
#if DEBUG
        OnMessageReceived = async _ => { await Task.Run(() => { Debug.WriteLine("====>  JWT Message received"); }); },

        OnTokenValidated = async _ => { await Task.Run(() => { Debug.WriteLine("====>  JWT token validated"); }); },
#endif
        OnAuthenticationFailed = async context =>
        {
            await Task.Run(() =>
            {
                Log.Error("There was a problem with JWT token validation. Exception='{@exception}'", context.Exception);
                Debug.WriteLine("====>  JWT token failed auth");
            });
        }
    };
}

static void ConfigureAppConfiguration(IConfigurationBuilder config, string[] args)
{
    var firstMandatoryProvider = config.Sources.FirstOrDefault();
    if (firstMandatoryProvider != null)
    {
        config.Sources.Clear();
        config.Sources.Add(firstMandatoryProvider);
    }

    config
        .AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true
        )
        .AddJsonFile(
            $"appsettings.{Environment.MachineName}.json",
            optional: true,
            reloadOnChange: true
        );

    config.AddEnvironmentVariables();
    config.AddCommandLine(args);
}