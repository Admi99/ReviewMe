namespace ReviewMe.Core.Services;

public class ManageDatabaseService : IManageDatabaseService
{
    private readonly IGenericRepository _genericRepository;

    public ManageDatabaseService(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task AddDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        await _genericRepository.AddDataAsync(entities);
    }

    public async Task DeleteDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        await _genericRepository.DeleteDataAsync(entities);
    }

    public async Task UpdateDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        await _genericRepository.UpdateDataAsync(entities);
    }

    public async Task AddNewDataAndUpdateExistingAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        await _genericRepository.AddNewDataAndUpdateExistingAsync(entities);
    }
}