using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;

namespace event_sourcing.Domain.PayrollLoan;
using EventStore.Client;

public class PayrollLoansRepository(EventStoreClient client)
{
    private const string StreamName = "payroll-loans";

    public async Task<Event> AppendEventAsync(Event @event, CancellationToken cancellationToken = default)
    {
        var eventData = new EventData(
            Uuid.NewUuid(),
            @event.Type,
            JsonSerializer.SerializeToUtf8Bytes(@event.Data),
            null);

        await client.AppendToStreamAsync(StreamName, StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);

        return @event with { Id = eventData.EventId.ToString() };
    }

    public async Task<List<Event>> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        var events = new List<Event>();
        var result = client.ReadStreamAsync(Direction.Forwards, StreamName, StreamPosition.Start, cancellationToken: cancellationToken);

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