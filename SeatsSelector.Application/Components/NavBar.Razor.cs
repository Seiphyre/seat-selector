using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SeatsSelector.Application.Services;

namespace SeatsSelector.Application.Components
{
    public class NavBarBase : ComponentBase
    {
        [Inject] protected IAccountService AccountService { get; set; }



        protected async void LogoutBtn_OnClick(MouseEventArgs e)
        {
            await AccountService.Logout();
        }
    }
}
