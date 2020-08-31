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
                //TODO: rather use SetProperty()

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
                //TODO: rather use SetProperty()

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
                //TODO: rather use SetProperty()

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
                //TODO: rather use SetProperty()

                _showLoading = value;
                RaisePropertyChanged(() => ShowLoading);
            }
        }

        MvxCommand _currentLocationCommand;
        public MvxCommand CurrentLocationCommand
        {
            // TODO :: every the getter fires, a new command will be created;
            // rather (1) instantiate the command in the Constructor or
            // (2) use lazy instantiation in getter
            get => new MvxCommand(CurrentLocationButtonClicked);

            set => _currentLocationCommand = value;
        }

        public void CurrentLocationButtonClicked() {
            SelectedCityName = "Select City Name";
            DisplayCurrentLocationWeatherAsync();
        }

        // TODO: where is this used? Consider naming it _placemark so as not to confuse with local params
        Placemark placemark = new Placemark();
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

        // TODO :: should be private
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

        // TODO :: should be private
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
