using NUnit.Framework;
using WeatherPro.Core.ViewModels;
using WeatherPro.Core.Services;
using Moq;
using MvvmCross.Tests;
using AutoFixture;
using Xamarin.Essentials;


namespace WeatherPro.UnitTest
{
    [TestFixture]
    public class MainViewModelTests : MvxIoCSupportingTest
    {
        private MainViewModel _mainViewModel;
        private Mock<IWeatherService> _weatherService;
        private Mock<ILocationService> _locationService;

        readonly Fixture _fixture;
        public MainViewModelTests()
        {
            _fixture = new Fixture();
        }


        MainViewModel mainViewModel => new MainViewModel(_weatherService.Object, _locationService.Object);

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

        }

        [Test]//RaisePropertyChanged
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
