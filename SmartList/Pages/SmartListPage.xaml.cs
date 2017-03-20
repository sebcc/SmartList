using System.Linq;
using System.Threading;
using PCLStorage;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace SmartList
{
    public partial class SmartListPage : ContentPage
    {
        public SmartListPage ()
        {
            InitializeComponent ();
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            if (this.BindingContext == null)
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                this.BindingContext = new ItemsViewModel (
                    locator,
                    DependencyService.Get<ILocalNotification> (),
                    new LoadItemsCommand (FileSystem.Current.LocalStorage),
                    new DeleteItemCommand (FileSystem.Current.LocalStorage));
            }

            var itemsViewModel = this.lstItems.BindingContext as ItemsViewModel;
            if (itemsViewModel == null)
            {
                this.lstItems.BindingContext = this.BindingContext;
            }
            else
            {
                itemsViewModel.Reset ();
            }

            itemsViewModel.LoadCommand.Execute (null);

            this.lstItems.ItemTapped += (sender, e) => {
                if (e == null) return; // has been set to null, do not 'process' tapped event
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

        }

        protected override void OnDisappearing ()
        {
            var itemsViewModel = this.lstItems.BindingContext as ItemsViewModel;
            if (itemsViewModel != null)
            {
                itemsViewModel.Cancel ();
            }

            base.OnDisappearing ();
        }

        void Handle_Clicked (object sender, System.EventArgs e)
        {
            var viewModel = new AddItemViewModel (FileSystem.Current.LocalStorage, new CategoriesService ());
            var addItemPage = new AddItemPage ();

            addItemPage.BindingContext = viewModel;
            Navigation.PushAsync (addItemPage, true);
        }
    }
}
