using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace SmartList
{
    public interface IGeolocatorService
    {
        Task<Position> GetPositionAsync (CancellationToken token);
    }
}
