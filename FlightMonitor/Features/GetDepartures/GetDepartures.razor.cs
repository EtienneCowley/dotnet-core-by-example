using Microsoft.AspNetCore.SignalR.Client;

namespace FlightMonitor.Features.GetDepartures;

public partial class GetDepartures
{
    private GetDeparturesViewModel? _model;
    private HubConnection? _hubConnection = null;

    protected override async Task OnInitializedAsync()
    {
        _model = await Mediator.Send(new GetDeparturesQuery());

        var hubUrl = Configuration["SignalRHubUrl"];
        _hubConnection = new HubConnectionBuilder().WithUrl(hubUrl!).Build();
        _hubConnection.On("RefreshFlightList", async () =>
        {
            try
            {
                var newModel = await Mediator.Send(new GetDeparturesQuery());
                await InvokeAsync(() =>
                {
                    _model = newModel;
                    StateHasChanged(); // Ensure this is inside InvokeAsync
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing flight list: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        });

        await _hubConnection.StartAsync();
    }
}