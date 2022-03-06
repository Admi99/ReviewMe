namespace ReviewMe.Core.Infrastructures;

internal interface IBusinessLogicAuthorizationService
{
    Task AuthorizeAsync(string policyName);
    Task AuthorizeAsync<T>(string policyName, T? requestToAuthorize);
}