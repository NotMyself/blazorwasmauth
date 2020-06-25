using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWasmAuth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Twitch", options.ProviderOptions);

                options.ProviderOptions.DefaultScopes.Clear();
                options.ProviderOptions.DefaultScopes.Add("openid user:read:email");

                options.ProviderOptions.ResponseType = "token id_token";

                // This is the default login-callback path
                //options.AuthenticationPaths.LogInCallbackPath = "https://localhost:5001/authentication/login-callback";
            });

            await builder.Build().RunAsync();
        }
    }
}
