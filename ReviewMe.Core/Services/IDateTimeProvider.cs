namespace ReviewMe.Core.Services;

public interface IDateTimeProvider
{
    DateTimeOffset Now();
}