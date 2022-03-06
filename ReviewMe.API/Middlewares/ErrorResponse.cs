using Newtonsoft.Json;
using ReviewMe.Core.Exceptions;

namespace ReviewMe.API.Middlewares;

public class ErrorResponse
{
    [JsonIgnore]
    public ErrorType ErrorType { get; }

    public string ErrorMessage { get; }

    protected ErrorResponse(ErrorType errorType, string errorMessage)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public static ErrorResponse Create(ErrorType errorType, string errorMessage)
    {
        return new ErrorResponse(errorType, errorMessage);
    }
}