using NUnit.Framework;
using WeatherPro.Core.Services;
using Xamarin.Essentials;
using MvvmCross.Tests;
using Moq;

namespace WeatherPro.Tests
{
    public class WeatherServiceTests : MvxIoCSupportingTest
    {
        private Mock<IWeatherService> _weatherService;

        [SetUp]
        public void init()
        {
            base.ClearAll();
            _weatherService = new Mock<IWeatherService>();
            Ioc.RegisterSingleton<IWeatherService>(_weatherService.Object);
        }


        [Test]
        public void GetWeatherInfoAsyncTest_WhenCalled_ItShoudNotNull()
        {
            Placemark placemark = new Placemark
            {
                Locality = "Pune",
                CountryName = "India"
            };

            var exp = _weatherService.Object.GetWeatherInfoAsync(placemark);
            Assert.IsNotNull(exp.AsyncState, "async response is null");
        }


        [Test]
        public void GetCities_WhenCalled_ItShouldNotBeNull()
        {
            var Cities = _weatherService.Object.GetCities();
            Assert.IsNotNull(Cities,"Cities object is null");
        }

        [Test]
        public void GetCities_WhenCalled_CountShouldGreaterThanZero()
        {
            var Cities = _weatherService.Object.GetCities();
            Assert.Greater(Cities.Count, 0);
        }

        [Test]
        public async System.Threading.Tasks.Task GetWeatherInfoAsync_WhenCalled_ItShouldGetSucessResult()
        {
            Placemark placemark = new Placemark
            {
                Locality = "Pimpri-Chinchwad",
                CountryName = "India"
            };

            var result = await _weatherService.Object.GetWeatherInfoAsync(placemark);
            Assert.IsTrue(result.Sucessful, "result error cause");
            
        }
    }
}
