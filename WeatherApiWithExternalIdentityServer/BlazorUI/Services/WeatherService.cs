using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorUI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeather()
        {
            var rtn = await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(
                await _httpClient.GetStreamAsync($"/api/WeatherForecast"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return rtn;
        }
    }
}
