using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewMe.Infrastructure.DbStorage.Repository;

namespace ReviewMe.Infrastructure.DbStorage;

public static class DiConfigDbStorage
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReviewMeDbContext>(options =>
        {
            //Debugger.Launch(); // Do not delete - helping to debug entity FW
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddSingleton<IGenericRepository, GenericRepository>();

        services.AddScoped<IEmployeesRepository, EmployeesRepository>();

        services.AddScoped<IAssessmentsRepository, AssessmentsRepository>();

        services.AddScoped<IAssessmentReviewerRepository, AssessmentReviewerRepository>();

        services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); },
            AppDomain.CurrentDomain.GetAssemblies());

    }
}