namespace FlightMonitor.Features.EditFlight;

/// <summary>
/// Gets the flight to edit with the given id.
/// </summary>
/// <param name="Id">The id (<see cref="Guid"/>) of the flight.</param>
public record GetFlightToEditRequest(Guid Id) : IRequest<EditFlightViewModel>;

public class GetFlightToEditRequestHandler(IDbContextFactory<AppDbContext> contextFactory)
    : IRequestHandler<GetFlightToEditRequest, EditFlightViewModel>
{
    public async Task<EditFlightViewModel> Handle(GetFlightToEditRequest request, CancellationToken cancellationToken)
    {
        var dbContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var flight = await dbContext.Flights.SingleAsync(f => f.Id == request.Id, cancellationToken);
        
        return new EditFlightViewModel(
            flight.Id,
            flight.FlightNumber, 
            flight.Destination,
            flight.ScheduledDeparture,
            flight.ActualDeparture,
            flight.Status);
    }
}