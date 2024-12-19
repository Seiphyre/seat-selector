using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SeatsSelector.Application.Services;
using SeatsSelector.Shared.Models.Rooms;
using SeatsSelector.Shared.Models.Seats;
using SeatsSelector.Shared.Models.Users;
using System;
using System.Text;

namespace SeatsSelector.Application.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] protected IWebAPI WebAPI { get; set; }
        [Inject] protected IAccountService AccountService { get; set; }

        protected Room _room = null;
        protected List<User> _users = null;
        protected User _me = null;
        protected bool _isInitialized = false;


        // -----------------------------------------------

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _room = await WebAPI.GetRoom(1);
            _users = await WebAPI.GetUsers();
            _me = await AccountService.GetCurrentUser();

            Console.WriteLine(_me.DisplayName);
            Console.WriteLine(_me.DisplayName);

            _isInitialized = true;
		}

        protected string GenerateCSV()
        {
            var csv = new StringBuilder();

            csv.AppendLine("Name, Seat");

            foreach (var user in _users)
            {
                csv.AppendLine($"{user.DisplayName},{user?.Seat?.Name ?? "--"}");
            }

            return csv.ToString();
        }

        protected async Task Refresh()
        {
            _room = await WebAPI.GetRoom(1);
            _users = await WebAPI.GetUsers();

            StateHasChanged();
        }

        protected async Task DownloadCsv()
        {
            var csvContent = GenerateCSV();  // Generate CSV content

            // Call JavaScript function to download CSV
            await JS.InvokeVoidAsync("downloadFile", "student-seats.csv", csvContent);
        }

        protected async void Seat_OnClick(Seat seat)
        {
            if (seat == null || seat.IsOccupied)
                return;

            try
            {
                await WebAPI.AssignUserToSeat(seat.Id, _me.Id);
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.Message);

                if (exception.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorMessage = "This seat is already taken";
                }
                else
                {
                    ErrorMessage = "Network error. Please try again.";
                }

                ErrorModalIsVisible = true;
            }
            finally
            {
                await Refresh();
            }
        }

        protected bool ErrorModalIsVisible = false;
        protected string ErrorMessage;
    
        public void ErrorModal_OnClose()
        {
            ErrorModalIsVisible = false;

            StateHasChanged();
        }
    }
}
