using EventStore.Client;

namespace event_sourcing.Infrastructure.AppStartup;

public static class ServicesExtensions
{
    public static IServiceCollection AddEventStoreDb(this IServiceCollection services)
    {
        // TODO: migrate to environment variables and secrets
        string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
        var settings = EventStoreClientSettings.Create(connectionString);
        var eventStoreClient = new EventStoreClient(settings);
        
        return services.AddSingleton(eventStoreClient);
    }
}