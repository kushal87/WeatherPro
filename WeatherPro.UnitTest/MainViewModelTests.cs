using System;
using NUnit.Framework;
using Xamarin.Essentials;
using WeatherPro.Core.ViewModels;
using WeatherPro.Core.Services;
using Moq;
namespace WeatherPro.UnitTest
{
    [TestFixture]
    public class MainViewModelTests
    {
        private MainViewModel _mainViewModel;
        private Mock<IWeatherService> _weatherService;
        private Mock<ILocationService> _locationService;

        [SetUp]
        public void Setup()
        {
            _weatherService = new Mock<IWeatherService>();
            _locationService = new Mock<ILocationService>();
            _mainViewModel = new MainViewModel(_weatherService.Object, _locationService.Object);
        }

        [Test]
        public void GetCurruntLocation_WhenCalled_LocationShouldNotNull()
        {
            // Arrange
            Location location = new Location
            {
                Latitude = 18.602933,
               Longitude = 73.7612999 
            };

           var exp = _locationService.Setup(x => x.GetCurrentLocationAsync()).ReturnsAsync(location);

            Assert.IsNotNull(exp);
              
        }
    }
}
