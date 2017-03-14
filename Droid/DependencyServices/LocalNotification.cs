using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Lang;
using String = System.String;
using NotificationCompat = Android.Support.V4.App.NotificationCompat;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using SmartList.Droid;

[assembly: Xamarin.Forms.Dependency (typeof (LocalNotification))]

namespace SmartList.Droid
{
    public class LocalNotification : ILocalNotification
    {
        public void Notify (string title, string message)
        {
            var context = Application.Context;
            var resultIntent = new Intent (context, typeof (MainActivity));
            resultIntent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var pending = PendingIntent.GetActivity (context, 0,
                resultIntent,
                PendingIntentFlags.CancelCurrent);

            var builder =
                new Notification.Builder (context)
                    .SetContentTitle (title)
                    .SetContentText (message)
                    .SetSmallIcon (Resource.Drawable.icon)
                    .SetDefaults (NotificationDefaults.All);

            builder.SetContentIntent (pending);

            var notification = builder.Build ();

            var manager = NotificationManager.FromContext (context);
            manager.Notify (1337, notification);

        }
    }
}
