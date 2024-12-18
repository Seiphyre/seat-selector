using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SeatsSelector.Application.Services;
using SeatsSelector.Shared.Models.Users;

namespace SeatsSelector.Application.Components
{
    public class NavBarBase : ComponentBase
    {
        [Inject] protected IAccountService AccountService { get; set; }
        protected User _me { get; set; }
        protected bool isInitialized { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            _me = await AccountService.GetCurrentUser();

            isInitialized = true;
        }

        protected async void LogoutBtn_OnClick(MouseEventArgs e)
        {
            await AccountService.Logout();
        }
    }
}
