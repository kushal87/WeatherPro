using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherPro.Core.Services
{
    public interface ILocationService
    {
        public Task<Location> GetCurrentLocationAsync();
        public Task<Placemark> GetCityNameAsync(Location location);
        Placemark GetCityNameAsync(Task<Location> location);
    }
}
