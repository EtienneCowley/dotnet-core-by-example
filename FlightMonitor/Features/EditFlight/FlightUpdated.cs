using FlightMonitor.Infrastructure;
using FlightMonitor.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FlightMonitor.Features.EditFlight;

public record FlightEditViewModel(
    Guid Id, 
    string Flight,
    string Destination, 
    DateTime Scheduled,
    DateTime Actual,
    FlightStatus Status);

public record FlightUpdatedNotification(Guid Id) : INotification;

public class FlightUpdatedNotificationHandler(IHubContext<FlightHub> hubContext)
    : INotificationHandler<FlightUpdatedNotification>
{
    public async Task Handle(FlightUpdatedNotification notification, CancellationToken cancellationToken)
    {
        // Send a message to all clients to refresh their flight list
        await hubContext.Clients.All.SendAsync("RefreshFlightList", cancellationToken);
    }
}
