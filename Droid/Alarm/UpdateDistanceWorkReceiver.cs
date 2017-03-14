using System;
using Android.App;
using Android.Content;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace SmartList.Droid
{
    [BroadcastReceiver]
    public class UpdateDistanceWorkReceiver : BroadcastReceiver
    {
        LocalNotification localNotification;

        public UpdateDistanceWorkReceiver ()
        {
            this.localNotification = new LocalNotification ();
        }

        public override void OnReceive (Context context, Intent intent)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var itemsViewModel = new ItemsViewModel (locator, this.localNotification);
            itemsViewModel.LoadCommand.Execute (null);
        }
    }
}
