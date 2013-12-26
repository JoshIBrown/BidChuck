using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using BCWeb.Models.Shared;
using Newtonsoft.Json;
using System.Web;
using System.Configuration;

namespace BCWeb.Helpers
{
    public class GeoLocator
    {
        public void GetFromAddress(string address, string city, string state, string postalcode, Action<RootObject> callback)
        {
            string key = ConfigurationManager.AppSettings["bingMapsKey"].ToString();

            // http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?includeNeighborhood=includeNeighborhood&maxResults=maxResults&key=BingMapsKey
            // http://msdn.microsoft.com/en-us/library/ff701714.aspx

            string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/US/{2}/{3}/{1}/{0}?key={4}", address, city, state, postalcode, key);

            string response = new WebClient().DownloadString(uri);

            if (callback != null && response != string.Empty)
            {
                RootObject dser = JsonConvert.DeserializeObject<RootObject>(response);
                callback(dser);
            }

        }
    }
}