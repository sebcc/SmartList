using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using PCLStorage;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public class ItemsViewModel : BaseViewModel
    {
        private ICommandResult<List<Item>> loadItemsCommand;
        private ICommandResult<Item> deleteCommand;
        private CancellationTokenSource cancellationTokenSource;

        private bool isRefreshing = false;
        private Position position = null;

        public ObservableCollection<ItemViewModel> Items
        {
            get;
            set;
        }

        readonly IGeolocator geolocator;

        public ItemsViewModel (
            IGeolocator geolocator,
            ILocalNotification localNotification,
            ICommandResult<List<Item>> loadItemsCommand,
            ICommandResult<Item> deleteCommand)
        {
            this.loadItemsCommand = loadItemsCommand;
            this.deleteCommand = deleteCommand;
            this.cancellationTokenSource = new CancellationTokenSource ();

            this.deleteCommand.Executed += (sender, e) => {
                var itemFromList = this.Items.First (i => i.Id == this.deleteCommand.Result.Id);
                this.Items.Remove (itemFromList);
            };

            this.geolocator = geolocator;
            this.Items = new ObservableCollection<ItemViewModel> ();
            this.loadItemsCommand.Executed += (sender, e) => {
                this.IsRefreshing = false;
                this.Items.Clear ();

                var placeApi = new GooglePlacesApi ();
                foreach (var item in this.loadItemsCommand.Result)
                {
                    var itemViewModel = new ItemViewModel (
                        item,
                        placeApi,
                        localNotification,
                        deleteCommand,
                        new CategoriesService (),
                        new ApplicationState ());

                    this.Items.Add (itemViewModel);
                }

                this.geolocator.GetPositionAsync (token: this.cancellationTokenSource.Token).ContinueWith ((arg) => {
                    this.position = arg.Result;
                    foreach (var item in this.Items)
                    {
                        item.UpdatePosition (this.position);
                    }
                });

            };
        }

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                OnPropertyChanged ();
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return this.loadItemsCommand;
            }
        }

        public void Cancel ()
        {
            this.cancellationTokenSource.Cancel ();
            foreach (var item in this.Items)
            {
                item.Cancel ();
            }
        }

        public void Reset ()
        {
            this.cancellationTokenSource = new CancellationTokenSource ();
            foreach (var item in this.Items)
            {
                item.Reset ();
            }
        }
    }
}
