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
            get => _selectedCityName;

            set
            {
                DisplayCityWeatherAsync(locality: SelectedCityName);
                SetProperty(ref _selectedCityName, value); 
            }           
        }

        private string _pickerDefaultTitle;
        public String PickerDefaultTitle
        {
            get => _pickerDefaultTitle;            
            set => SetProperty(ref _pickerDefaultTitle, value);
        }

        private Weather _weather;
        public Weather Weather
        {
            get => _weather;
            set => SetProperty(ref _weather, value); 
        }

        private bool _showLoading = false;
        public bool ShowLoading
        {
            get => _showLoading;
            set => SetProperty(ref _showLoading, value);
        }

        MvxCommand _currentLocationCommand;
        public MvxCommand CurrentLocationCommand
        {
            // TODO :: every the getter fires, a new command will be created;
            // rather (1) instantiate the command in the Constructor or
            // (2) use lazy instantiation in getter            
            get
            {
                _currentLocationCommand = _currentLocationCommand ?? new MvxCommand(CurrentLocationButtonClicked);
                return _currentLocationCommand;
            }

            set => _currentLocationCommand = value;
        }

        public void CurrentLocationButtonClicked() {
            SelectedCityName = "Select City Name";
            DisplayCurrentLocationWeatherAsync();
        }

        private ILocationService _locationService { get; }
        private IWeatherService _weatherService { get; }
        

        // TODO: need unit tests for this class
        public MainViewModel(IWeatherService weatherService, ILocationService locationService)
        {
            _locationService = locationService;
            _weatherService = weatherService;

            CityList = _weatherService.GetCities();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            PickerDefaultTitle = "Select City Name";
            DisplayCurrentLocationWeatherAsync();         
        }
        
        private async void DisplayCurrentLocationWeatherAsync()
        {
            try
            {
                var location = await _locationService.GetCurrentLocationAsync();
                Placemark placemark = await _locationService.GetCityNameAsync(location);
                DisplayCityWeatherAsync(placemark);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void DisplayCityWeatherAsync(Placemark placemark = null, String locality = "") {

            Placemark _placemark = placemark ?? new Placemark();

            if (locality.Length > 0) {
                _placemark.Locality = locality;
            }
         
            ShowLoading = true;
            var result = await _weatherService.GetWeatherInfoAsync(_placemark);
            ShowLoading = false;

            if (result.Sucessful)
            {
                Weather = Weather.FromJson((string)result.Response);
            }
        }
    }
}
