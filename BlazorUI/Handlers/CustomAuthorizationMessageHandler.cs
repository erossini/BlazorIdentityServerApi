using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Handlers
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) :
            base(provider, navigation)
        {
            ConfigureHandler(authorizedUrls: new[] { "https://localhost:44323" },
                scopes: new[] { "weather_api" });
        }
    }
}