using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using event_sourcing.Domain.PayrollLoan.Events;

namespace event_sourcing.Domain.PayrollLoan;
using EventStore.Client;

public class PayrollLoansRepository(EventStoreClient client)
{
    private const string StreamNamePrefix = "payrollloan";

    private static string StreamName(Guid streamId) => $"{StreamNamePrefix}-{streamId}";

    public async Task AppendEventAsync(Event @event, CancellationToken cancellationToken = default)
    {
        var eventData = new EventData(
            Uuid.FromGuid(@event.StreamId),
            @event.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes(@event),
            null);

        await client.AppendToStreamAsync(StreamName(@event.StreamId), StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);
    }

    public async Task<List<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default)
    {
        var result = client.ReadAllAsync(Direction.Forwards, Position.Start, StreamFilter.Prefix(StreamNamePrefix), cancellationToken: cancellationToken);
        var events = new List<Event>();
        await foreach (var resolvedEvent in result)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) continue;
            
            var @event = JsonSerializer.Deserialize<Event>(resolvedEvent.Event.Data.Span);
            events.Add(@event);
        }
        
        return events;
    }
    
    public async Task<Maybe<PayrollLoanProjection>> GetPayrollLoanAsync(Guid loanId)
    {
        // Load events from the event store
        var events = await GetEventsByLoanIdAsync(loanId);

        // Reconstruct the PayrollLoan aggregate from events
        var payrollLoan = new PayrollLoanProjection();
        foreach (var @event in events)
        {
            payrollLoan.Apply(@event);
        }

        return payrollLoan;
    }

    private async Task<List<Event>> GetEventsByLoanIdAsync(Guid loanId)
    {
        var result = client.ReadStreamAsync(Direction.Forwards, StreamName(loanId), StreamPosition.Start);
        var events = new List<Event>();
        await foreach (var resolvedEvent in result)
        {
            var @event = JsonSerializer.Deserialize<Event>(resolvedEvent.Event.Data.Span);
            events.Add(@event);
        }
        
        return events;
    }
}