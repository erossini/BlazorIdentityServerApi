using BlazorUI.Handlers;
using BlazorUI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            #region AddHttpClient per service
            builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddHttpClient<IWeatherService, WeatherService>(
                client => client.BaseAddress = new Uri("https://localhost:44323/api"))
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
			#endregion
			#region IHttpClientFactory
			builder.Services.AddHttpClient("weatherAPI", cl =>
			{
				cl.BaseAddress = new Uri("https://localhost:44323/api/WeatherForecast");
			})
			.AddHttpMessageHandler(sp =>
			{
				var handler = sp.GetService<AuthorizationMessageHandler>()
				.ConfigureHandler(
					authorizedUrls: new[] { "https://localhost:44323" },
					scopes: new[] { "company-employee" }
				 );
				return handler;
			});
			builder.Services.AddScoped(
				sp => sp.GetService<IHttpClientFactory>().CreateClient("weatherAPI"));
			#endregion

			builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
