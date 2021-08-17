using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorUI.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeather();
    }
}