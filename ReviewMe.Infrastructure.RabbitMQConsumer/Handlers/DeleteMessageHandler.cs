using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using ReviewMe.Core.Services;
using ReviewMe.Infrastructure.RabbitMQConsumer.Handlers.Shared;
using ReviewMe.Infrastructure.RabbitMQConsumer.Helpers;
using ReviewMe.Infrastructure.RabbitMQConsumer.Objects;
using ReviewMe.Infrastructure.RabbitMQConsumer.Services;

namespace ReviewMe.Infrastructure.RabbitMQConsumer.Handlers;

public class DeleteMessageHandler<TCoreType, TRabbitMqType>
    : HandlerBase<TCoreType, TRabbitMqType>, IAsyncMessageHandler
    where TCoreType : class, IEntity
    where TRabbitMqType : class, IEntityRo
{
    private readonly IManageDatabaseService _manageDatabase;
    private readonly IMapper _mapper;
    private readonly IRabbitMqStateService _rabbitMqStateService;
    private readonly ILogger _logger;

    public DeleteMessageHandler(IManageDatabaseService manageDatabase, IMapper mapper, IRabbitMqStateService rabbitMqStateService, ILogger<DeleteMessageHandler<TCoreType, TRabbitMqType>> logger)
    {
        _manageDatabase = manageDatabase;
        _mapper = mapper;
        _rabbitMqStateService = rabbitMqStateService;
        _logger = logger;
    }

    public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
    {
        try
        {
            if (_rabbitMqStateService.IsBlockedBecauseOfDataSynch)
            {
                _logger.LogError(Messages.DeleteBlockBecauseOfDataSynchErrorMessage);
                throw new Exception(Messages.DeleteBlockBecauseOfDataSynchErrorMessage);
            }
            var coreObject = DeserializeAndMapData(eventArgs, _mapper);
            await _manageDatabase.DeleteDataAsync(coreObject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error occurred");
            throw;
        }
    }
}