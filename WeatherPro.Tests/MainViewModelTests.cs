using NUnit.Framework;
using WeatherPro.Core.ViewModels;
using WeatherPro.Core.Services;
using Moq;
using MvvmCross.Tests;
using Xamarin.Essentials;
using System;

namespace WeatherPro.Tests
{
    [TestFixture]
    public class MainViewModelTests: MvxIoCSupportingTest
    {
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

        [TearDown]
        public void Cleanup()
        {
            _weatherService = null;
            _locationService = null;            
        }

        [Test]
        public void DisplayCurrentLocationWeatherAsyncTest()
        {
            try
            {
                var location = _locationService.Object.GetCurrentLocationAsync();
                Placemark placemark = _locationService.Object.GetCityNameAsync(location);
                Assert.IsNotNull(placemark, "Placemark should not null");                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void DisplayCityWeatherAsyncTest(Placemark placemark = null, String locality = "")
        {
            Placemark _placemark = placemark ?? new Placemark();

            Assert.IsNotNull(_placemark, "placemark not null");
            if (locality.Length > 0)
            {
                _placemark.Locality = locality;
                Assert.IsNotEmpty(_placemark.Locality, "locality is not empty");
            }
        }

    }
}
