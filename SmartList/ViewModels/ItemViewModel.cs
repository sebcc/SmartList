using System;
using System.Collections.Generic;
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
        readonly IApplicationState applicationState;
        readonly ICommandResult<double> closestPlacesCommand;

        public ItemViewModel (
            Item item,
            ICommandResult<double> closestPlacesCommand,
            ILocalNotification localNotification,
            ICommand deleteCommand,
            ICategoriesService categoryService,
            IApplicationState applicationState)
        {
            this.closestPlacesCommand = closestPlacesCommand;
            this.applicationState = applicationState;
            this.cancellationTokenSource = new CancellationTokenSource ();
            this.categoryService = categoryService;
            this.localNotification = localNotification;
            this.position = null;
            this.item = item;
            this.deleteCommand = deleteCommand;

            this.closestPlacesCommand.Executed += (sender, e) => {
                this.Distance = this.closestPlacesCommand.Result;

                var distanceForNotification = 100;

#if DEBUG
                distanceForNotification = 1000;
#endif

                if (this.Distance <= distanceForNotification && !this.applicationState.GetState ())
                {
                    this.localNotification.Notify ("Close-by", "Don't forget about your item " + this.Name, this.item.Id);
                }
            };
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

        public ICommandResult<double> LoadClosestPlace
        {
            get
            {
                return this.closestPlacesCommand;
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