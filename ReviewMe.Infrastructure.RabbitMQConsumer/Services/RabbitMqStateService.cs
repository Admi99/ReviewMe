namespace ReviewMe.Infrastructure.RabbitMQConsumer.Services;

public class RabbitMqStateService : IRabbitMqStateService
{
    private Guid RequestCorrelationId { get; set; }

    public bool IsBlockedBecauseOfDataSynch { get; set; }

    public Guid Block()
    {
        IsBlockedBecauseOfDataSynch = true;
        var requestCorrelationId = Guid.NewGuid();
        RequestCorrelationId = requestCorrelationId;
        return requestCorrelationId;
    }

    public void UnBlock(Guid responseCorrelationId)
    {
        if (DoesRequestMatchResponse(responseCorrelationId))
            IsBlockedBecauseOfDataSynch = false;
    }

    public bool DoesRequestMatchResponse(Guid responseCorrelationId)
        => RequestCorrelationId == responseCorrelationId;
}