using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using CommunityToolkit.Mvvm.Input;
using MapDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MapDemo.ViewModels
{
    public partial class MapViewModel : BaseViewModel
    {
        public ObservableCollection<Place> Places { get; } = new();

        [ObservableProperty]
        bool isReady;

        [ObservableProperty]
        ObservableCollection<Place> bindablePlaces;

        private CancellationTokenSource cts;
        private IGeolocation geolocation;
        private IGeocoding geocoding;

        public MapViewModel(IGeolocation geolocation, IGeocoding geocoding)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
        }

        [RelayCommand]
        private async Task GetCurrentLocationAsync()
        {
            try
            {
                cts = new CancellationTokenSource();

                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                var location = await geolocation.GetLocationAsync(request, cts.Token);
                var placemarks = await geocoding.GetPlacemarksAsync(location);
                var address = placemarks?.FirstOrDefault()?.AdminArea;

				Places.Clear();
				
                var place = new Place()
                {
                    Location = location, 
                    Address = address,
                    Description = "Current Location"
                };

                Places.Add(place);

                var placeList = new List<Place>() { place };
				BindablePlaces = new ObservableCollection<Place>(placeList);
                IsReady = true;
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        [RelayCommand]
        private void DisposeCancellationToken()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }
    }
}
