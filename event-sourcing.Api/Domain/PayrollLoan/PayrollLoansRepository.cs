using System.Text;
using System.Text.Json;
using System.Threading;

namespace event_sourcing.Domain.PayrollLoan;
using EventStore.Client;

public class PayrollLoansRepository(EventStoreClient client)
{

    public async Task AppendEventAsync(string streamName, Event @event)
    {
        var eventData = new EventData(
            Uuid.NewUuid(),
            @event.Type,
            JsonSerializer.SerializeToUtf8Bytes(@event.Data),
            null);

        await client.AppendToStreamAsync(streamName, StreamState.Any, new[] { eventData });
    }

    public async Task<List<Event>> GetEventsAsync(string streamName, CancellationToken cancellationToken = default)
    {
        var events = new List<Event>();
        var result = client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start, cancellationToken: cancellationToken);

        await foreach (var resolvedEvent in result)
        {
            events.Add(new Event
            {
                Id = resolvedEvent.Event.EventId.ToString(),
                Type = resolvedEvent.Event.EventType,
                Data = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray()),
                CreatedAt = resolvedEvent.Event.Created
            });
        }

        return events;
    }
}