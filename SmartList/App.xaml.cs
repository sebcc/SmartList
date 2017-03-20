using PCLStorage;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public partial class App : Application
    {
        IBackgroundWorker backgroundWorker;
        IApplicationState applicationState;

        public App ()
        {
            InitializeComponent ();

            applicationState = new ApplicationState ();
            var navigationPage = new NavigationPage (new SmartListPage ());

            MainPage = navigationPage;

            DependencyService.Register<ILocalNotification> ();
            this.backgroundWorker = DependencyService.Get<IBackgroundWorker> ();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
            applicationState.SetState (true);
            this.backgroundWorker.StopAll ();
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
            applicationState.SetState (false);
            this.backgroundWorker.StartUpdateDistanceWork (15 * 60 * 1000);
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
            applicationState.SetState (true);
            this.backgroundWorker.StopAll ();

        }
    }
}
