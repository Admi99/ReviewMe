using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection;
using ReviewMe.Core.Services;
using ReviewMe.Infrastructure.RabbitMQConsumer.Handlers;
using ReviewMe.Infrastructure.RabbitMQConsumer.Helpers;
using ReviewMe.Infrastructure.RabbitMQConsumer.Objects;
using ReviewMe.Infrastructure.RabbitMQConsumer.Services;

namespace ReviewMe.Infrastructure.RabbitMQConsumer;

public static class DiConfigRabbitMqConsumer
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        var rabbitMqSection = configuration.GetSection("RabbitMq");
        var exchangeSection = configuration.GetSection("RabbitMqExchange");

        services.AddHostedService<RabbitMqConsumerHostedService>();
        services.AddSingleton<IRabbitMqStateService, RabbitMqStateService>();

        services.AddRabbitMqClient(rabbitMqSection)
            .AddConsumptionExchange(Constants.ExchangeName, exchangeSection)
            .AddSingleton<IManageDatabaseService, ManageDatabaseService>()
            .AddAsyncMessageHandlerSingleton<UpdateMessageHandler<Employee, EmployeeRo>>("public.Update.*")
            .AddAsyncMessageHandlerSingleton<AddMessageHandler<Employee, EmployeeRo>>("public.Insert.*")
            .AddAsyncMessageHandlerSingleton<DeleteMessageHandler<Employee, EmployeeRo>>("public.Delete.*")
            .AddAsyncMessageHandlerSingleton<SynchronizationMessageHandler<Employee, EmployeeRo>>(
                "public.GetAllEmployeesResponse");

        services.AddSingleton<ISynchronizeData, SynchronizeDataService>();

        services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); },
            AppDomain.CurrentDomain.GetAssemblies());
    }
}