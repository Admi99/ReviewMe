namespace ReviewMe.Core.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now() => DateTimeOffset.Now;
}