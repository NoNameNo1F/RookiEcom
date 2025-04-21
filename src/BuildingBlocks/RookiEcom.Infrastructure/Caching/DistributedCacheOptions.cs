namespace RookiEcom.Infrastructure.Caching;

public class DistributedCacheOptions
{
    public string Provider { get; set; }
    public RedisOptions Redis { get; set; }
}

public class RedisOptions
{
    public string Configuration { get; set; }
    public string InstanceName { get; set; }
}
