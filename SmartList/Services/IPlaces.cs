using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartList
{
    public interface IPlaces
    {
        Task<List<Place>> FetchPlacesAsync (double latitude, double longitude, string placeType, CancellationToken cancellationToken);

    }
}
