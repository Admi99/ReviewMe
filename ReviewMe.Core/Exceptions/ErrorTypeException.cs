namespace ReviewMe.Core.Exceptions;

public class ErrorTypeException : Exception
{
    public ErrorTypeException(ErrorType errorType, string message) : base(message)
    {
        ErrorType = errorType;
    }

    public ErrorType ErrorType { get; }
}