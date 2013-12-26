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

        /// <summary>
        /// get's geo locaiton data from bing maps rest api
        /// </summary>
        /// <param name="address">US street address</param>
        /// <param name="city">US city</param>
        /// <param name="state">US state</param>
        /// <param name="postalcode">US postal code</param>
        /// <param name="callback">callback function</param>
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

        public void GetFromCityStateZip(string city, string state, string postalcode, Action<RootObject> callback)
        {
            string key = ConfigurationManager.AppSettings["bingMapsKey"].ToString();

            // http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?includeNeighborhood=includeNeighborhood&maxResults=maxResults&key=BingMapsKey
            // http://msdn.microsoft.com/en-us/library/ff701714.aspx

            string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/?adminDistrict={1}&postalCode={2}&locality={1}&countryRegion=US&key={3}", city, state, postalcode, key);

            string response = new WebClient().DownloadString(uri);

            if (callback != null && response != string.Empty)
            {
                RootObject dser = JsonConvert.DeserializeObject<RootObject>(response);
                callback(dser);
            }

        }
    }
}