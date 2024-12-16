using Microsoft.AspNetCore.Components;
using SeatsSelector.Application.Services;
using SeatsSelector.Shared.Models.Rooms;
using SeatsSelector.Shared.Models.Seats;
using System.Text;

namespace SeatsSelector.Application.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject] protected IWebAPI WebAPI { get; set; }

        protected Room _room = null;
        protected bool _isInitialized = false;


        // -----------------------------------------------

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _room = await WebAPI.GetRoom(1);

            _isInitialized = true;
		}

        protected void Seat_OnClick(Seat seat)
        {
            if (seat == null)
                return;

            seat.IsOccupied = !seat.IsOccupied;

            StateHasChanged();
        }
    }
}
