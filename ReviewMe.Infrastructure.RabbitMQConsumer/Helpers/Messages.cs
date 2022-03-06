namespace ReviewMe.Infrastructure.RabbitMQConsumer.Helpers;

public static class Messages
{
    public const string InsertBlockBecauseOfDataSynchErrorMessage = "Data are synchronizing, standard insert cannot be comleted, message is queued !";
    public const string DeleteBlockBecauseOfDataSynchErrorMessage = "Data are synchronizing, standard delete cannot be comleted, message is queued !";
    public const string UpdateBlockBecauseOfDataSynchErrorMessage = "Data are synchronizing, standard update cannot be comleted, message is queued !";
    public const string RequestDoesntMatchResponseErrorMessage = "Response doesn't match request";
}