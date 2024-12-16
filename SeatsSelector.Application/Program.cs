using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SeatsSelector.Application.Services;

namespace SeatsSelector.Application
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // -- HttpClient

			builder.Services.AddScoped(sp => new HttpClient
			{
				BaseAddress = new Uri(builder.HostEnvironment.IsDevelopment() ? "https://localhost:7234/" : "https://seat-selector-webapi.azurewebsites.net/")
			});

			// -- Custom services

			builder.Services.AddScoped<IWebAPI, WebAPI>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            // -- Authentication

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            // -- Third party services

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
