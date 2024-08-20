namespace FlightMonitor.Features.GetDepartures;

public record DepartureViewModel(
    Guid Id, 
    string Time,
    string Flight, 
    string Destination,
    string Status);

public record ListDeparturesViewModel(IEnumerable<DepartureViewModel> Departures);

/// <summary>
/// Gets a list of the top 20 departures.
/// </summary>
public record GetDeparturesRequest : IRequest<ListDeparturesViewModel>;

public class GetDeparturesRequestHandler(IDbContextFactory<AppDbContext> contextFactory) 
    : IRequestHandler<GetDeparturesRequest, ListDeparturesViewModel>
{
    public async Task<ListDeparturesViewModel> Handle(GetDeparturesRequest request, CancellationToken cancellationToken)
    {
        var dbContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var flights = await dbContext.Flights
            .Take(20)
            .OrderBy(c => c.ScheduledDeparture)
            .ToListAsync(cancellationToken);
        
        var viewModels = flights.Select(flight => 
            new DepartureViewModel(
                flight.Id,
                flight.ActualDeparture.ToString("hh:mm tt"),
                flight.FlightNumber, 
                flight.Destination, 
                flight.Status.ToString()))
            .ToList();
        
        return new ListDeparturesViewModel(viewModels);
    }
}