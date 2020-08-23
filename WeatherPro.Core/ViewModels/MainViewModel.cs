using System;
using MvvmCross.ViewModels;
using Xamarin.Essentials;
using WeatherPro.Core.Services;
using WeatherPro.Core.Model;
using MvvmCross.Commands;
using System.Collections.Generic;

namespace WeatherPro.Core.ViewModels
{
    public class MainViewModel :  MvxViewModel
    {

        public List<string> CityList { get; set; }
      
        private string _selectedCityName;
        public String SelectedCityName
        {
            get { return _selectedCityName; }

            set
            {
                _selectedCityName = value;
                if (_selectedCityName == null)
                    return;

                placemark.Locality = SelectedCityName;
                DisplayCityWeatherAsync(placemark);
                SelectedCityName = null; 
            }
        }

        private Weather _weather;
        public Weather Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                RaisePropertyChanged(() => Weather);
            }
        }

        private bool _showLoading = false;
        public bool ShowLoading
        {
            get => _showLoading;
            set
            {             
                _showLoading = value;
                RaisePropertyChanged(() => ShowLoading);
            }
        }

        MvxCommand _currentLocationCommand;
        public MvxCommand CurrentLocationCommand
        {
            get => new MvxCommand(DisplayCurrentLocationWeatherAsync);

            set => _currentLocationCommand = value;
        }

        Placemark placemark = new Placemark();
        private ILocationService LocationService { get; }
        private IWeatherService WeatherService { get; }
                
        public MainViewModel(IWeatherService weatherService, ILocationService locationService)
        {
            LocationService = locationService;
            WeatherService = weatherService;
            CityList = WeatherService.GetCities();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            DisplayCurrentLocationWeatherAsync();         
        }

        public async void DisplayCurrentLocationWeatherAsync()
        {
            try
            {
                var location = await LocationService.GetCurrentLocationAsync();
                Placemark placemark = await LocationService.GetCityNameAsync(location);
                DisplayCityWeatherAsync(placemark);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void DisplayCityWeatherAsync(Placemark placemark) {

            if (placemark != null)
            {
                ShowLoading = true;
                var result = await WeatherService.GetWeatherInfoAsync(placemark);
                ShowLoading = false;

                if (result.Sucessful)
                {
                    Weather = Weather.FromJson((string)result.Response);
                }
            }
        }
    }
}
