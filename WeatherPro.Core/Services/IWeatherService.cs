using Xamarin.Essentials;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherPro.Core.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherInfoAsync(Placemark placemark);
        List<string> GetCities();
    }
}
