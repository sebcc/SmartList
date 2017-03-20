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
        public void Notify (string title, string message, string itemId)
        {
            var context = Application.Context;
            var resultIntent = new Intent (context, typeof (MainActivity));
            resultIntent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var pending = PendingIntent.GetActivity (context, 0,
                resultIntent,
                PendingIntentFlags.CancelCurrent);

            var pickedUpIntent = new Intent ("PickedUp");
            pickedUpIntent.PutExtra ("itemId", itemId);
            var pendingPickedUpIntent = PendingIntent.GetBroadcast (context, 0, pickedUpIntent, PendingIntentFlags.CancelCurrent);
            var builder =
                new Notification.Builder (context)
                    .SetContentTitle (title)
                    .SetContentText (message)
                    .SetSmallIcon (Resource.Drawable.icon)
                    .SetDefaults (NotificationDefaults.All)
                    .SetVisibility (NotificationVisibility.Public);

            builder.SetContentIntent (pending);
            var pickedUpAction = new Notification.Action (Resource.Drawable.abc_btn_check_material, "Picked Up", pendingPickedUpIntent);
            builder.AddAction (pickedUpAction);

            var notification = builder.Build ();
            notification.Flags = NotificationFlags.AutoCancel;
            var manager = NotificationManager.FromContext (context);
            manager.Notify (1337, notification);
        }
    }
}
