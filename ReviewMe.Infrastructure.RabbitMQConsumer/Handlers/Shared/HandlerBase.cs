using RabbitMQ.Client.Events;
using ReviewMe.Infrastructure.RabbitMQConsumer.Objects;
using System.Text;
using System.Text.Json;

namespace ReviewMe.Infrastructure.RabbitMQConsumer.Handlers.Shared;

public class HandlerBase<TCoreType, TRabbitMqType>
    where TCoreType : IEntity
    where TRabbitMqType : IEntityRo
{
    protected static IReadOnlyCollection<TCoreType> DeserializeAndMapData(BasicDeliverEventArgs eventArgs, IMapper mapper)
    {
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        var deserializedMessage = JsonSerializer.Deserialize<IReadOnlyCollection<TRabbitMqType>>(message);
        var coreObject = mapper.Map<IReadOnlyCollection<TCoreType>>(deserializedMessage);
        return coreObject;
    }
}