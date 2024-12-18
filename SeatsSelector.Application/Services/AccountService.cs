using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SeatsSelector.Shared.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SeatsSelector.Application.Services
{
    public interface IAccountService
    {
        Task Login(string username, string password);
        Task Logout();

        Task<User> GetCurrentUser();
    }

    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IWebAPI _webAPI;

        public async Task<User> GetCurrentUser()
        {
            string token = await _localStorage.GetItemAsync<string>("token");

            return User.FromToken(token);
        }



        // --------------------------------------------------------

        public AccountService(IWebAPI webAPI, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _webAPI = webAPI;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }



        // --------------------------------------------------------

        public async Task Login(string username, string password)
        {
            try
            {
                string token = await _webAPI.Authenticate(username, password);

                // --

                if (!string.IsNullOrWhiteSpace(token))
                    await _localStorage.SetItemAsync("token", token);
            }
            finally
            {
                await _authStateProvider.GetAuthenticationStateAsync();
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            await _authStateProvider.GetAuthenticationStateAsync();
        }



        // --------------------------------------------------------

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var claimsPrincipal = new ClaimsPrincipal();

        //    // -- 

        //    var user = LoadUserFromBrowser();

        //    if (user is not null)
        //    {
        //        string token = await _webAPI.Authenticate(user.Username, user.Password);

        //        if (!string.IsNullOrWhiteSpace(token))
        //            claimsPrincipal = CreateClaimsPrincipalFromToken(token);
        //    }

        //    return new AuthenticationState(claimsPrincipal);
        //}

        //private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        //{
        //    var authenticationState = await task;

        //    if (authenticationState is not null)
        //    {
        //        CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        //    }
        //}

        // --------------------------------------------------------

        //private User LoadUserFromBrowser()
        //{
        //    var claimsPrincipal = CreateClaimsPrincipalFromToken(_authenticationDataMemoryStorage.Token);
        //    var user = User.FromClaimsPrincipal(claimsPrincipal);

        //    return user;
        //}

        //private void SaveUserToBrowser(string token)
        //{
        //    _authenticationDataMemoryStorage.Token = token;
        //}

        // --

        //private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var identity = new ClaimsIdentity();

        //    if (tokenHandler.CanReadToken(token))
        //    {
        //        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        //        identity = new ClaimsIdentity(jwtSecurityToken.Claims);
        //    }

        //    return new ClaimsPrincipal(identity);
        //}
    }
}
