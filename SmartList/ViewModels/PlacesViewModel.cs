using System;
using System.Windows.Input;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public class PlacesViewModel : BaseViewModel
    {
        readonly IPlaces placesServices;
        readonly IGeolocator geolocator;

        public PlacesViewModel (IPlaces placesServices, IGeolocator geolocator)
        {
            this.geolocator = geolocator;
            this.placesServices = placesServices;
        }

        public ICommand LoadPlaces
        {
            get;
            set;
        }
    }
}
