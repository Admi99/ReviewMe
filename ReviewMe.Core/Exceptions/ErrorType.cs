namespace ReviewMe.Core.Exceptions;

public enum ErrorType
{
    GeneralRequestValidation = 400,
    Authentication = 401,
    Authorization = 403,
    ResourceNotFound = 404,
    GenericServerError = 500,
    GenericEmailError = 4301
}