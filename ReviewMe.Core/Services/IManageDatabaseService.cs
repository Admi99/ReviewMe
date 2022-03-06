namespace ReviewMe.Core.Services;

public interface IManageDatabaseService
{
    Task AddDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task DeleteDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task UpdateDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task AddNewDataAndUpdateExistingAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
}