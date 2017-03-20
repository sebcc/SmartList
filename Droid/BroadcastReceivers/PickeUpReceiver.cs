using System;
using Android.App;
using Android.Content;

namespace SmartList.Droid
{
    [BroadcastReceiver (Enabled = true)]
    [IntentFilter (new [] { "PickedUp" })]
    public class PickedUpReceiver : BroadcastReceiver
    {
        public override void OnReceive (Context context, Intent intent)
        {
            if (intent != null)
            {
                var itemId = intent.GetStringExtra ("itemId");

                var deleteCommand = new DeleteItemCommand (PCLStorage.FileSystem.Current.LocalStorage);
                deleteCommand.Execute (itemId);

                var manager = NotificationManager.FromContext (context);
                manager.CancelAll ();
            }
        }
    }
}
