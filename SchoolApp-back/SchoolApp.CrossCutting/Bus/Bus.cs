using MediatR;
using Serilog;

namespace SchoolApp.CrossCutting.Bus;

public class Bus(IMediator mediator, ILogger logger) : IBus
{
    public async Task<object> SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
    {
        logger.Information("Calling Command Bus | Class: SchoolApp.CrossCutting.Bus | Method: SendCommandAsync");

        return await mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        logger.Information("Calling Event Bus | Class: SchoolApp.CrossCutting.Bus | Method: SendEventAsync");

        await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }

    public async Task<object> SendQueryAsync<TQuery>(TQuery query, CancellationToken cancellationToken)
    {
        logger.Information("Calling Query Bus | Class: SchoolApp.CrossCutting.Bus | Method: SendQueryAsync");

        return await mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }
}