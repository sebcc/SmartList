using System;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using PCLStorage;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public class ItemViewModel : BaseViewModel
    {
        readonly Item item;
        double distance = double.NaN;
        readonly IPlaces placesService;
        Position position;
        readonly ILocalNotification localNotification;
        readonly ICommand deleteCommand;
        readonly ICategoriesService categoryService;
        private CancellationTokenSource cancellationTokenSource;

        public ItemViewModel (
            Item item,
            IPlaces placesService,
            ILocalNotification localNotification,
            ICommand deleteCommand,
            ICategoriesService categoryService)
        {
            this.cancellationTokenSource = new CancellationTokenSource ();
            this.categoryService = categoryService;
            this.localNotification = localNotification;
            this.position = null;
            this.placesService = placesService;
            this.item = item;
            this.deleteCommand = deleteCommand;
        }

        public string Name
        {
            get
            {
                return this.item.Name;
            }
        }

        public string Id
        {
            get
            {
                return this.item.Id;
            }
        }

        public double Distance
        {
            get
            {
                return this.distance;
            }
            private set
            {
                this.distance = value;
                OnPropertyChanged ();
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return this.deleteCommand;
            }
        }

        public void UpdatePosition (Position position)
        {
            this.position = position;

            if (this.position != null && !string.IsNullOrEmpty (this.item.CategoryId))
            {
                var category = this.categoryService.FetchCategories ().FirstOrDefault ((arg) => arg.Id == this.item.CategoryId);
                if (category != null)
                {
                    var places = this.placesService.FetchPlacesAsync (position.Latitude, position.Longitude, category.GooglePlaceName, cancellationTokenSource.Token).Result;

                    var closestPlace = places.OrderBy (p => DistanceHelper.DistanceBetween (position.Latitude, position.Longitude, p.Latitude, p.Longitude)).FirstOrDefault ();
                    this.Distance = DistanceHelper.DistanceBetween (position.Latitude, position.Longitude, closestPlace.Latitude, closestPlace.Longitude);

                    if (this.Distance < 100)
                    {
                        this.localNotification.Notify ("Close-by", "Don't forget about your item " + this.Name);
                    }
                }
                else
                {
                    this.Distance = double.NaN;
                }

            }
            else
            {
                this.Distance = double.NaN;
            }

        }

        public void Cancel ()
        {
            this.cancellationTokenSource.Cancel ();
        }

        public void Reset ()
        {
            this.cancellationTokenSource = new CancellationTokenSource ();
        }
    }
}