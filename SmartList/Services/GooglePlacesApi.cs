﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartList
{
    public class GooglePlacesApi : IPlaces
    {

        public async Task<List<Place>> FetchPlacesAsync (double latitude, double longitude, string placeType, CancellationToken cancellationToken)
        {
            var places = new List<Place> ();
            using (var client = new HttpClient ())
            {
                var response = await client.GetAsync (string.Format ("https://maps.googleapis.com/maps/api/place/radarsearch/json?location={0},{1}&radius=1000&type={2}&key={3}", latitude, longitude, placeType, ""), HttpCompletionOption.ResponseContentRead, cancellationToken);
                var content = await response.Content.ReadAsStringAsync ();
                dynamic deserialized = JsonConvert.DeserializeObject<dynamic> (content);

                foreach (var item in deserialized.results)
                {
                    var place = new Place ();
                    place.Id = item.id;
                    place.Name = item.name;
                    place.Latitude = item.geometry.location.lat;
                    place.Longitude = item.geometry.location.lng;

                    places.Add (place);
                }
            }

            return places;

        }

    }
}

