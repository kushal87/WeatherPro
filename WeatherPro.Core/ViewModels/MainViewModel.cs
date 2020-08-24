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


        private string _pickerDefaultTitle;
        public String PickerDefaultTitle
        {
            get { return _pickerDefaultTitle; }

            set
            {
                _pickerDefaultTitle = value;
                if (_pickerDefaultTitle == null)
                    return;

                RaisePropertyChanged(() => PickerDefaultTitle);
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
            get => new MvxCommand(CurrentLocationButtonClicked);

            set => _currentLocationCommand = value;
        }

        public void CurrentLocationButtonClicked() {
            SelectedCityName = "Select City Name";
            DisplayCurrentLocationWeatherAsync();
        }

        Placemark placemark = new Placemark();
        private ILocationService _locationService { get; }
        private IWeatherService _weatherService { get; }
                
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

        public async void DisplayCurrentLocationWeatherAsync()
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

        public async void DisplayCityWeatherAsync(Placemark placemark) {

            if (placemark != null)
            {
                ShowLoading = true;
                var result = await _weatherService.GetWeatherInfoAsync(placemark);
                ShowLoading = false;

                if (result.Sucessful)
                {
                    Weather = Weather.FromJson((string)result.Response);
                }
            }
        }
    }
}
