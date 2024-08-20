namespace FlightMonitor.Features.EditFlight;

public record EditFlightViewModel(
    Guid Id, 
    string Flight,
    string Destination, 
    DateTime Scheduled,
    DateTime Actual,
    FlightStatus Status);

/// <summary>
/// Sends a notification to all clients to inform them that the flight with the given id as updated.
/// </summary>
/// <param name="Id">The id (<see cref="Guid"/>) of the flight that has changed.</param>
public record FlightUpdatedNotification(Guid Id) : INotification;

public class FlightUpdatedNotificationHandler(IHubContext<FlightHub> hubContext)
    : INotificationHandler<FlightUpdatedNotification>
{
    public async Task Handle(FlightUpdatedNotification notification, CancellationToken cancellationToken) => 
        await hubContext.Clients.All.SendAsync("RefreshFlightList", cancellationToken);
}
