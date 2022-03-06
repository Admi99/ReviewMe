using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using ReviewMe.Core.Services;
using ReviewMe.Infrastructure.RabbitMQConsumer.Handlers.Shared;
using ReviewMe.Infrastructure.RabbitMQConsumer.Helpers;
using ReviewMe.Infrastructure.RabbitMQConsumer.Objects;
using ReviewMe.Infrastructure.RabbitMQConsumer.Services;

namespace ReviewMe.Infrastructure.RabbitMQConsumer.Handlers;

public class SynchronizationMessageHandler<TCoreType, TRabbitMqType>
    : HandlerBase<TCoreType, TRabbitMqType>, IAsyncMessageHandler
    where TCoreType : class, IEntity
    where TRabbitMqType : class, IEntityRo
{
    private readonly IRabbitMqStateService _rabbitMqStateService;
    private readonly IManageDatabaseService _manageDatabaseService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SynchronizationMessageHandler(IManageDatabaseService manageDatabaseService, IMapper mapper, IRabbitMqStateService rabbitMqStateService, ILogger<SynchronizationMessageHandler<TCoreType, TRabbitMqType>> logger)
    {
        _manageDatabaseService = manageDatabaseService;
        _mapper = mapper;
        _rabbitMqStateService = rabbitMqStateService;
        _logger = logger;
    }

    public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
    {
        try
        {
            var responseCorrelationId = eventArgs.BasicProperties.CorrelationId;
            Guid.TryParse(responseCorrelationId, out var responseCorrelationIdGuid);

            if (_rabbitMqStateService.DoesRequestMatchResponse(responseCorrelationIdGuid))
            {
                var coreObjects = DeserializeAndMapData(eventArgs, _mapper);
                await _manageDatabaseService.AddNewDataAndUpdateExistingAsync(coreObjects);

                _rabbitMqStateService.UnBlock(responseCorrelationIdGuid);
            }
            else
            {
                _logger.LogError(Messages.RequestDoesntMatchResponseErrorMessage);
                throw new Exception(Messages.RequestDoesntMatchResponseErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error occurred");
            throw;
        }

    }

}