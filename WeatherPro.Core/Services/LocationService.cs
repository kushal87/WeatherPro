using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherPro.Core.Services
{
    public class LocationService : ILocationService
    {
        
        public async Task<Location> GetCurrentLocationAsync()
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);                
                location = await Geolocation.GetLocationAsync(request);
                
                if (location != null)
                {
                    Console.WriteLine("Current Location: ",location.ToString());                                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: ",ex.ToString());
            }

            return location;
        }


        public async Task<Placemark> GetCityNameAsync(Location location)
        {
            if (location != null)
            {
                try
                {
                    var places = await Geocoding.GetPlacemarksAsync(location);
                    var currentPlace = places?.FirstOrDefault();

                    if (currentPlace != null)
                    {
                        return currentPlace;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CityName Error", ex.ToString());
                }
            }

            return null;
        }
    }
}