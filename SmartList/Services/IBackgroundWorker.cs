using System;
namespace SmartList
{
    public interface IBackgroundWorker
    {
        void StartUpdateDistanceWork (long repeatInMilliseconds);
        void StopAll ();
    }
}
