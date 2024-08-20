using FlightMonitor.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightMonitor.Features.GetDepartures;

public record DepartureViewModel(
    Guid Id, 
    string Time,
    string Flight, 
    string Destination,
    string Status);

public record GetDeparturesViewModel(IEnumerable<DepartureViewModel> Departures);

public record GetDeparturesQuery : IRequest<GetDeparturesViewModel>;

public class GetDeparturesRequestHandler(IDbContextFactory<AppDbContext> contextFactory) 
    : IRequestHandler<GetDeparturesQuery, GetDeparturesViewModel>
{
    public async Task<GetDeparturesViewModel> Handle(GetDeparturesQuery request, CancellationToken cancellationToken)
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
        
        return new GetDeparturesViewModel(viewModels);
    }
}