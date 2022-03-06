namespace ReviewMe.Infrastructure.DbStorage.Comparers;

public class EntityIdComparer<T> : IEqualityComparer<T> where T : IEntity
{
    public bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Id == y.Id;
    }

    public int GetHashCode(T obj)
        => obj.Id.GetHashCode();
}