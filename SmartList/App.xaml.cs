using PCLStorage;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public partial class App : Application
    {
        IBackgroundWorker backgroundWorker;
        public App ()
        {
            InitializeComponent ();

            var navigationPage = new NavigationPage (new SmartListPage ());

            MainPage = navigationPage;

            DependencyService.Register<ILocalNotification> ();
            this.backgroundWorker = DependencyService.Get<IBackgroundWorker> ();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
            this.backgroundWorker.StopAll ();
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
            this.backgroundWorker.StartUpdateDistanceWork (15 * 60 * 1000);
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
            this.backgroundWorker.StopAll ();

        }
    }
}
