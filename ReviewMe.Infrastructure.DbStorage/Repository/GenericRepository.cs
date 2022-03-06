using Microsoft.Extensions.DependencyInjection;

namespace ReviewMe.Infrastructure.DbStorage.Repository;

internal class GenericRepository : IGenericRepository
{
    private readonly IMapper _mapper;
    private readonly IConfigurationProvider _mapperConfiguration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GenericRepository> _logger;

    public GenericRepository(IMapper mapper, IServiceProvider serviceProvider, IConfigurationProvider mapperConfiguration, ILogger<GenericRepository> logger)
    {
        _mapperConfiguration = mapperConfiguration;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task UpdateDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        var destTypeMap = GetDestTypeMap<T>();

        var db = GetDbContext();

        if (entities.Count == 0)
            _logger.LogError("RabbitMQ Entities list is empty");

        var entitiesMapped = entities
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        db.UpdateRange(entitiesMapped);
        await db.SaveChangesAsync();
    }

    public async Task AddDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        var destTypeMap = GetDestTypeMap<T>();

        var db = GetDbContext();

        if (entities.Count == 0)
            _logger.LogError("RabbitMQ Entities list is empty");

        var entitiesMapped = entities
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        await db.AddRangeAsync(entitiesMapped);
        await db.SaveChangesAsync();
    }

    public async Task AddNewDataAndUpdateExistingAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        var db = GetDbContext();
        var destTypeMap = GetDestTypeMap<T>();

        var allSavedEntities = GetAllRecords<T>();

        var entitiesToAdd = entities.Except(allSavedEntities, new EntityIdComparer<T>()).ToList();
        var entitiesToDelete = allSavedEntities.Except(entities, new EntityIdComparer<T>()).ToList();
        var entitiesToUpdate = entities.Except(entitiesToAdd, new EntityIdComparer<T>()).ToList();

        var entitiesToAddMapped = entitiesToAdd
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        var entitiesToDeleteMapped = entitiesToDelete
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        var entitiesToUpdateMapped = entitiesToUpdate
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        await db.AddRangeAsync(entitiesToAddMapped);
        db.RemoveRange(entitiesToDeleteMapped);
        db.UpdateRange(entitiesToUpdateMapped);

        await db.SaveChangesAsync();
    }

    private IReadOnlyCollection<T> GetAllRecords<T>() where T : class
    {
        var coreType = typeof(T);
        var destTypeMap = GetDestTypeMap<T>();
        var doType = destTypeMap.DestinationType;

        var db = GetDbContext();
        var dbSet = db
            .GetType()
            .GetMethods()
            .First(method => method.Name == nameof(DbContext.Set) && method.GetParameters().Length == 0)
            .MakeGenericMethod(doType)
            .Invoke(db, Array.Empty<object>()) as IEnumerable<object>;

        var result= dbSet!
            .Select(c => _mapper.Map(c, doType, coreType))
            .Cast<T>()
            .ToList();

         return result;
    }

    public async Task DeleteDataAsync<T>(IReadOnlyCollection<T> entities) where T : class, IEntity
    {
        var destTypeMap = GetDestTypeMap<T>();

        var db = GetDbContext();

        if (entities.Count == 0)
            _logger.LogError("RabbitMQ Entities list is empty");

        var entitiesMapped = entities
            .Select(c => _mapper.Map(c, typeof(T), destTypeMap.DestinationType));

        db.RemoveRange(entitiesMapped);
        await db.SaveChangesAsync();
    }

    private ReviewMeDbContext GetDbContext()
        => _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ReviewMeDbContext>();

    private TypeMap GetDestTypeMap<T>()
        => _mapperConfiguration
               .GetAllTypeMaps()
               .FirstOrDefault(item => item.SourceType == typeof(T))
           ?? throw new ApplicationException($"Auto mapper conf does not contain conf for '{typeof(T).Name}'");

}