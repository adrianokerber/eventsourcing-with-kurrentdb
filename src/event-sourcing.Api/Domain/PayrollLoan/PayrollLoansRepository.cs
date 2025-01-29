using System.Text.Json;
using CSharpFunctionalExtensions;
using event_sourcing.Domain.Shared;
using EventStore.Client;

namespace event_sourcing.Domain.PayrollLoan;

public class PayrollLoansRepository(EventStoreClient client)
{
    private const string StreamNamePrefix = "payrollloan";

    private static string StreamName(Guid streamId) => $"{StreamNamePrefix}-{streamId}";

    public async Task Save(PayrollLoan payrollLoan, CancellationToken cancellationToken)
    {
        foreach (var @event in payrollLoan.NewEvents)
        {
            await AppendEventAsync(@event, cancellationToken);
        }
    }

    private async Task AppendEventAsync(Event @event, CancellationToken cancellationToken = default)
    {
        var eventData = new EventData(
            Uuid.NewUuid(), // TODO: This is the idempotency key, learn a way to use it to ensure events are not duplicated
            @event.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes(@event),
            null);

        await client.AppendToStreamAsync(StreamName(@event.StreamId), StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);
    }

    // TODO: improve to use a projection instead of requesting all events to see the current state of all aggregates
    public async Task<List<PayrollLoan>> GetAllPayrollLoansAsync(CancellationToken cancellationToken = default)
    {
        var events = await GetAllEventsAsync(cancellationToken);
        var loanIdToEvents = events
            .GroupBy(e => e.StreamId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<PayrollLoan>();
        foreach (var (loanId, loanEvents) in loanIdToEvents)
        {
            var loan = new PayrollLoan(loanEvents);
            result.Add(loan);
        }

        return result;
    }

    private async Task<List<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default)
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
    
    public async Task<Maybe<PayrollLoan>> GetPayrollLoanByIdAsync(Guid loanId, CancellationToken cancellationToken = default)
    {
        // Load events from the event store
        var events = await GetEventsByLoanIdAsync(loanId, cancellationToken: cancellationToken);
        if (events.Count == 0)
            return Maybe<PayrollLoan>.None;

        // Reconstruct the PayrollLoan aggregate from events
        var payrollLoan = new PayrollLoan(events);
        return payrollLoan;
    }

    private async Task<List<Event>> GetEventsByLoanIdAsync(Guid loanId, CancellationToken cancellationToken = default)
    {
        var result = client.ReadStreamAsync(Direction.Forwards, StreamName(loanId), StreamPosition.Start, cancellationToken: cancellationToken);
        if (await result.ReadState == ReadState.StreamNotFound)
            return new List<Event>();

        var events = new List<Event>();
        await foreach (var resolvedEvent in result)
        {
            var @event = JsonSerializer.Deserialize<Event>(resolvedEvent.Event.Data.Span);
            events.Add(@event);
        }
        
        return events;
    }
}