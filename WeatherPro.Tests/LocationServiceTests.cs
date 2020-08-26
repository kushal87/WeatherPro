using Moq;
using MvvmCross.Tests;
using NUnit.Framework;
using WeatherPro.Core.Services;
using Xamarin.Essentials;

namespace WeatherPro.Tests
{
    public class LocationServiceTests : MvxIoCSupportingTest
    {
        private Mock<ILocationService> _locationService;

        [SetUp]
        public void init()
        {
            base.ClearAll();
            _locationService = new Mock<ILocationService>();
            Ioc.RegisterSingleton<ILocationService>(_locationService.Object);
        }

        [Test]
        public void GetCurrentLocationAsync_WhenCalled_LocationShouldNotNull()
        {
            Location location = _locationService.Object.GetCurrentLocationAsync().Result;
            Assert.NotNull(location, "location is null");
        }

        [Test]
        public void GetCityNameAsync_WhenCalled_PlacemarkShouldNotNull()
        {
            // Arrange
            Location location = new Location
            {
                Latitude = 18.602933,
                Longitude = 73.7612999
            };
            Placemark placemark = _locationService.Object.GetCityNameAsync(location).Result;                        
            AssertionException.Equals(placemark, null);
        }
    }
}
