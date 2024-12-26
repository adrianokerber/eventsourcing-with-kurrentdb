using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace event_sourcing.Infrastructure.AppStartup;

public static class EventStoreExtensions
{
    public static IServiceCollection AddEventStoreDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("EventStore:ConnectionString").Value;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("EventStore connection string is not configured");
        }

        var settings = EventStoreClientSettings.Create(connectionString);
        var client = new EventStoreClient(settings);
        
        services.AddSingleton(client);
        
        return services;
    }
}
