using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using SmartList.Droid;

[assembly: Xamarin.Forms.Dependency (typeof (BackgroundWorker))]

namespace SmartList.Droid
{
    public class BackgroundWorker : IBackgroundWorker
    {
        public void StartUpdateDistanceWork (long repeatInMilliseconds)
        {
            var context = Application.Context;
            var alarmIntent = new Intent (context, typeof (UpdateDistanceWorkReceiver));
            var pending = PendingIntent.GetBroadcast (context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = context.GetSystemService (Context.AlarmService).JavaCast<AlarmManager> ();
            alarmManager.SetRepeating (AlarmType.RtcWakeup, 0, repeatInMilliseconds, pending);
        }

        public void StopAll ()
        {
            var context = Application.Context;

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService (Context.AlarmService);

            Intent updateServiceIntent = new Intent (context, typeof (UpdateDistanceWorkReceiver));
            PendingIntent pendingUpdateIntent = PendingIntent.GetService (context, 0, updateServiceIntent, 0);

            // Cancel alarms
            try
            {
                alarmManager.Cancel (pendingUpdateIntent);
            }
            catch (Exception e)
            {
                var a = 4;
            }
        }
    }
}
