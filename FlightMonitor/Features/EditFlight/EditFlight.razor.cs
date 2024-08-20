using Microsoft.AspNetCore.Components;

namespace FlightMonitor.Features.EditFlight;

public partial class EditFlight
{
    [Parameter] public string? Id { get; set; }
    
    private EditFlightViewModel? Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if(Id is null)
            return;
        
        Model = await Mediator.Send(new GetFlightToEditRequest(Guid.Parse(Id)));
    }

    private async Task SetDelay()
    {
        if(Model is null || Id is null)
            return;
        
        var command = new SetDelayCommand(Model.Id, 30);
        await Mediator.Send(command);
        Model = await Mediator.Send(new GetFlightToEditRequest(Guid.Parse(Id)));
    }
}