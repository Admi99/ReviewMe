using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System.Text;
using Constants = ReviewMe.Infrastructure.RabbitMQConsumer.Helpers.Constants;

namespace ReviewMe.Infrastructure.RabbitMQConsumer.Services;

public class SynchronizeDataService : ISynchronizeData
{

    private readonly IRabbitMqStateService _rabbitMqStateService;
    private readonly IQueueService _queueService;

    public SynchronizeDataService(IRabbitMqStateService rabbitMqStateService, IQueueService queueService)
    {
        _rabbitMqStateService = rabbitMqStateService;
        _queueService = queueService;
    }

    public void SendRequestToRefreshTable(TableName tableName)
    {
        var requestCorrelationId = _rabbitMqStateService.Block();

        var props = SetBasicProperties(tableName, requestCorrelationId);

        _queueService.Send(
            Encoding.UTF8.GetBytes(string.Empty),
            props,
            Constants.ExchangeName,
            GetRequestRoutingKey(tableName));
    }

    private IBasicProperties SetBasicProperties(TableName tableName, Guid requestCorrelationId)
    {
        var props = _queueService.Channel.CreateBasicProperties();
        props.ReplyTo = GetResponseRoutingKey(tableName);
        props.CorrelationId = requestCorrelationId.ToString();

        return props;
    }

    private static string GetRequestRoutingKey(TableName table) => table switch
    {
        TableName.Employees => "public.GetAllEmployees",
        _ => throw new ArgumentOutOfRangeException(nameof(table), $"Not expected value: {table}")
    };

    private static string GetResponseRoutingKey(TableName table) => table switch
    {
        TableName.Employees => "public.GetAllEmployeesResponse",
        _ => throw new ArgumentOutOfRangeException(nameof(table), $"Not expected value: {table}")
    };
}