using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Essentials;
using WeatherPro.Core.Models;
using System.Web;

namespace WeatherPro.Core.Services
{
    public class WeatherService: IWeatherService
    {
        public async Task <WeatherResponse> GetWeatherInfoAsync(Placemark placeMark)
        {
            using (var client = new HttpClient())
            {                
                String placemark = $"{placeMark.Locality}, {placeMark.CountryName}";
                var htmlEncodedPlaceMark = HttpUtility.UrlEncode(placemark, System.Text.Encoding.UTF8);

                String uriString = Constant.OpenWeatherUrl + string.Format("{0}&appid={1}&units=metric", htmlEncodedPlaceMark, Constant.AppId);

                var uri = new Uri(uriString: uriString);
                var response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {
                    return new WeatherResponse { Response = await response.Content.ReadAsStringAsync() };
                }
                else
                    return new WeatherResponse { ErrorMessage = response.ReasonPhrase };
            }
        }

        public List<string> GetCities()
        {
            List<string> Cities = CityList.GetCities();
            return Cities;
        }
    }

    public class WeatherResponse
    {
        public bool Sucessful => ErrorMessage == null;

        public string ErrorMessage { get; set; }
        public string Response { get; set; }
        public bool Successful { get; internal set; }

        public object weather { get; set; }
    }
   
}
