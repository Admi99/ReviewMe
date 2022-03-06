namespace ReviewMe.Core.Infrastructures;

public interface IGenericRepository
{
    Task DeleteDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task UpdateDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task AddDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
    Task AddNewDataAndUpdateExistingAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity;
}