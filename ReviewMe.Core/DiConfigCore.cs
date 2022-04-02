using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewMe.Core.Authorization;
using ReviewMe.Core.Services;
using ReviewMe.Core.Services.CommandServices.AssessmentsService;
using ReviewMe.Core.Services.EmailNotificationServices;
using ReviewMe.Core.Services.QueryServices.ColleagueService;
using ReviewMe.Core.Services.QueryServices.EmployeesService;
using ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;
using ReviewMe.Core.Services.QueryServices.ReviewersService;
using ReviewMe.Core.Services.QueryServices.ReviewerTasksService;
using ReviewMe.Core.Settings;

namespace ReviewMe.Core;

public static class DiConfigCore
{
    // !!! All classes implementing interface should be internal
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var applicationSettingsSection = configuration.GetSection("ApplicationSettings");
        services.Configure<ApplicationSettings>(applicationSettingsSection);

        services.AddAuthorization(options => options.AddReviewmePolicies());

        services.AddScoped<IClaimsTransformation, RoleClaimsTransform>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IEmployeesService, EmployeesService>();
        services.AddScoped<IColleagueService, ColleagueService>();
        services.AddScoped<IAssessmentsService, AssessmentsService>();
        services.AddScoped<IReviewersService, ReviewersService>();
        services.AddScoped<IReviewerFeedbackService, ReviewerFeedbackService>();
        services.AddScoped<IEmailSendingService, EmailSendingService>();
        services.AddScoped<IAssessmentsNotificationService, AssessmentsNotificationService>();

        services.AddHttpClient();

        services.AddScoped<IHttpClientService, HttpClientService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IReviewerTaskService, ReviewerTaskService>();
    }
}