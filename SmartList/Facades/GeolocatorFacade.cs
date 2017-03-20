using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public class GeolocatorFacade : IGeolocatorService
    {
        readonly IGeolocator geolocator;
        Position lastPosition = null;
        DateTime lastChecked = DateTime.UtcNow;
        object lockObject = new object ();

        public GeolocatorFacade (IGeolocator geolocator)
        {
            this.geolocator = geolocator;
        }

        public async Task<Position> GetPositionAsync (CancellationToken token)
        {
            return await Task.Run (() => {
                lock (lockObject)
                {
                    if (this.lastPosition == null || (DateTime.UtcNow - lastChecked).TotalMinutes >= 5)
                    {
                        var newPosition = this.geolocator.GetPositionAsync (token: token);
                        newPosition.Wait ();
                        this.lastPosition = newPosition.Result;
                        this.lastChecked = DateTime.UtcNow;


                    }

                    return this.lastPosition;
                }
            }).ConfigureAwait (false);
        }
    }
}
