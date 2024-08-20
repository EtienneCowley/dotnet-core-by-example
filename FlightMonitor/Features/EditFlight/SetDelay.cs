namespace FlightMonitor.Features.EditFlight;

/// <summary>
/// This command delays a flight by 30 minutes, and sends a "<see cref="FlightUpdatedNotification"/>" to inform all clients.
/// </summary>
/// <param name="Id">The id (<see cref="Guid"/>) of the flight to delay.</param>
/// <param name="DelayInMinutes">The amount of minutes to delay the flight by.</param>
public record SetDelayCommand(Guid Id, uint DelayInMinutes) : IRequest;

public class SetDelayCommandHandler(AppDbContext dbContext, IPublisher publisher)
    : IRequestHandler<SetDelayCommand>
{
    public async Task Handle(SetDelayCommand request, CancellationToken cancellationToken)
    {
        var flight = await dbContext.Flights.SingleAsync(f => f.Id == request.Id, cancellationToken);
        
        flight.ActualDeparture = flight.ActualDeparture.AddMinutes(request.DelayInMinutes);
        flight.Status = FlightStatus.Delayed;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await publisher.Publish(new FlightUpdatedNotification(flight.Id), cancellationToken);
    }
}