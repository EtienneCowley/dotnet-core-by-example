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
    public async Task Handle(FlightUpdatedNotification notification, CancellationToken cancellationToken) => 
        await hubContext.Clients.All.SendAsync("RefreshFlightList", cancellationToken);
}
