
namespace Basket.Service.Infrastructure.Data.Redis;

public static class RedisExtensions
{
    public static void AddRedisCache(this IServiceCollection services, IConfigurationManager configuration)
    {
        var redisOptions = new RedisOptions();
        configuration.GetSection(RedisOptions.RedisSectionName).Bind(redisOptions);

        services.AddStackExchangeRedisCache(options => options.Configuration = redisOptions.Configuration);
    }
}
