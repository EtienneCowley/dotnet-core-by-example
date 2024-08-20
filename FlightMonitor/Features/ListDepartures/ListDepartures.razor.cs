using FlightMonitor.Features.GetDepartures;
using Microsoft.AspNetCore.Components;

namespace FlightMonitor.Features.ListDepartures;

public partial class ListDepartures
{
    private ListDeparturesViewModel? Model { get; set; }
    private HubConnection? HubConnection { get; set; }
    [Inject] private IMediator? Mediator { get; set; }
    [Inject] private IConfiguration? Configuration { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Mediator is null || Configuration is null)
            return;

        Model = await Mediator.Send(new GetDeparturesRequest());

        var hubUrl = Configuration["SignalRHubUrl"];
        HubConnection = new HubConnectionBuilder().WithUrl(hubUrl!).Build();
        HubConnection.On("RefreshFlightList", async () =>
        {
            try
            {
                var newModel = await Mediator.Send(new GetDeparturesRequest());
                await InvokeAsync(() =>
                {
                    Model = newModel;
                    StateHasChanged();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing flight list: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        });

        await HubConnection.StartAsync();
    }
}