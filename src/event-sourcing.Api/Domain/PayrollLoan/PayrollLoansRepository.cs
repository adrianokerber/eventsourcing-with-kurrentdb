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
            @event.Type,
            JsonSerializer.SerializeToUtf8Bytes(@event.Data),
            null);

        await client.AppendToStreamAsync(StreamName(@event.StreamId), StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);
    }

    public async Task<List<Event>> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        var events = new List<Event>();
        var result = client.ReadAllAsync(Direction.Forwards, Position.Start, StreamFilter.Prefix(StreamNamePrefix), cancellationToken: cancellationToken);

        await foreach (var resolvedEvent in result)
        {
            var @event = JsonSerializer.Deserialize<Event>(Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray()), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            events.Add(@event);
        }

        return events;
    }
    
    /*
    public async Task<Maybe<PayrollLoan>> GetPayrollLoanAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = client.ReadStreamAsync(Direction.Forwards, StreamNamePrefix, StreamPosition.Start, cancellationToken: cancellationToken);

        await foreach (var resolvedEvent in result)
        {
            var @event = new Event
            {
                Id = resolvedEvent.Event.EventId.ToString(),
                Type = resolvedEvent.Event.EventType,
                Data = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray()),
                CreatedAtUtc = resolvedEvent.Event.Created
            };

            if (@event.Type == "PayrollLoanCreated")
            {
                var data = JsonSerializer.Deserialize<PayrollLoanCreated>(@event.Data);
                return new PayrollLoan
                {
                    Id = @event.Id,
                    Amount = data.Amount,
                    InterestRate = data.InterestRate,
                    NumberOfInstallments = data.NumberOfInstallments,
                    CreatedAtUtc = @event.CreatedAtUtc
                };
            }
        }

        return Maybe<PayrollLoan>.None;
    }
    */
}