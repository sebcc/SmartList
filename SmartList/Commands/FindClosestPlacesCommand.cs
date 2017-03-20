using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public class FindClosestPlacesCommand : ICommandResult<double>
    {
        readonly IPlaces placesApi;
        readonly IGeolocatorService geoLocator;
        readonly CancellationToken cancellationToken;
        readonly ICategoriesService categoryService;
        private double distance;

        public FindClosestPlacesCommand (
            IPlaces placesApi,
            IGeolocatorService geoLocator,
            ICategoriesService categoryService,
            CancellationToken cancellationToken)
        {
            this.categoryService = categoryService;
            this.cancellationToken = cancellationToken;
            this.geoLocator = geoLocator;
            this.placesApi = placesApi;
        }

        public double Result
        {
            get
            {
                return this.distance;
            }
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler Executed;

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public void Execute (object parameter)
        {
            var item = parameter as Item;

            this.geoLocator.GetPositionAsync (token: this.cancellationToken).ContinueWith ((arg1) => {
                if (arg1.Result != null && !string.IsNullOrEmpty (item.CategoryId))
                {
                    var category = this.categoryService.FetchCategories ().FirstOrDefault ((arg) => arg.Id == item.CategoryId);
                    if (category != null)
                    {
                        var places = this.placesApi.FetchPlacesAsync (arg1.Result.Latitude, arg1.Result.Longitude, category.GooglePlaceName, cancellationToken);
                        places.Wait ();

                        var closestPlace = places.Result.OrderBy (p => DistanceHelper.DistanceBetween (arg1.Result.Latitude, arg1.Result.Longitude, p.Latitude, p.Longitude)).FirstOrDefault ();
                        if (closestPlace != null)
                        {
                            this.distance = DistanceHelper.DistanceBetween (arg1.Result.Latitude, arg1.Result.Longitude, closestPlace.Latitude, closestPlace.Longitude);
                        }
                        else
                        {
                            this.distance = double.NaN;
                        }
                    }

                    if (this.Executed != null)
                    {
                        this.Executed.Invoke (this, new EventArgs ());
                    }
                }
            });

        }
    }
}
