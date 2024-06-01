namespace TesteCacheRedisDecorator.Attributes
{
    public class CachedAttribute(bool useCache) : Attribute
    {
        public bool UseCache { get; } = useCache;
    }
}
