using MvvmCross.Tests;
using NUnit.Framework;
using WeatherPro.Core.ViewModels;
using WeatherPro.Core.Services;
using Xamarin.Essentials;
using Moq;

namespace WeatherPro.UnitTest
{
    public class WeatherServiceTest: MvxIoCSupportingTest
    {

        private MainViewModel _mainViewModel;
        private Mock<IWeatherService> _weatherService;
        private Mock<ILocationService> _locationService;
                
        [SetUp]
        public void init()
        {
            base.ClearAll();
             _weatherService = new Mock<IWeatherService>();            
            _locationService = new Mock<ILocationService>();
            Ioc.RegisterSingleton<IWeatherService>(_weatherService.Object);
            Ioc.RegisterSingleton<ILocationService>(_locationService.Object);
        }


        [Test]
        public void GetWeatherInfoAsyncTest_WhenCalled_ItShoudNotNull()
        {
            Placemark placemark = new Placemark
            {
                Locality = "Pune",
                CountryName = "India"
            };

            var exp = _weatherService.Setup(x => x.GetWeatherInfoAsync(placemark)).ReturnsAsync(new WeatherResponse());

            Assert.IsNotNull(exp);
        }


        [Test]
        public void GetCities_WhenCalled_ItShouldNotBeNull()
        {
            var weatherService = new WeatherService();
            var Cities = weatherService.GetCities();
            Assert.IsNotNull(Cities);
        }

        [Test]
        public void GetCities_WhenCalled_CountShouldGreaterThanZero()
        {
            var weatherService = new WeatherService();
            var Cities = weatherService.GetCities();
            Assert.Greater(Cities.Count, 0);
        }

        [Test]
        public async System.Threading.Tasks.Task GetWeatherInfoAsync_WhenCalled_ItShouldGetSucessResult()
        {
            var weatherService = new WeatherService();

            Placemark placemark = new Placemark
            {
                Locality = "Pimpri-Chinchwad",
                CountryName = "India"
            };

            var result = await weatherService.GetWeatherInfoAsync(placemark);
            Assert.IsTrue(result.Sucessful);
        }
    }
}
