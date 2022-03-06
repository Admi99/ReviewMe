namespace ReviewMe.Infrastructure.RabbitMQConsumer.Services;

public interface IRabbitMqStateService
{
    bool IsBlockedBecauseOfDataSynch { get; }
    Guid Block();
    void UnBlock(Guid responseCorrelationId);
    bool DoesRequestMatchResponse(Guid responseCorrelationId);

}