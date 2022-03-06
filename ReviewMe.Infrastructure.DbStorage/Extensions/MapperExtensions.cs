namespace ReviewMe.Infrastructure.DbStorage.Extensions;

public static class MapperExtensions
{
    public static T Map<T>(this object? obj, IMapper mapper) => mapper.Map<T>(obj);
}