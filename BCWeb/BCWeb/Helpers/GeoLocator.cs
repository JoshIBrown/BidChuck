using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using BCWeb.Models.Shared.BingMaps;
using Newtonsoft.Json;
using System.Web;
using System.Configuration;
using System.Data.Spatial;

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
        public DbGeography GetFromAddress(string address, string city, string state, string postalcode)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["bingMapsKey"].ToString();

                // http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?includeNeighborhood=includeNeighborhood&maxResults=maxResults&key=BingMapsKey
                // http://msdn.microsoft.com/en-us/library/ff701714.aspx

                string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/US/{2}/{3}/{1}/{0}?key={4}", address, city, state, postalcode, key);

                string response = new WebClient().DownloadString(uri);


                RootObject dser = JsonConvert.DeserializeObject<RootObject>(response);
                Dictionary<string, double> coords = getLatLong(dser);
                return getDbGeography(coords["lat"], coords["lng"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbGeography GetFromCityStateZip(string city, string state, string postalcode)
        {
            string key = ConfigurationManager.AppSettings["bingMapsKey"].ToString();

            // http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?includeNeighborhood=includeNeighborhood&maxResults=maxResults&key=BingMapsKey
            // http://msdn.microsoft.com/en-us/library/ff701714.aspx

            try
            {
                string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/?adminDistrict={1}&postalCode={2}&locality={0}&countryRegion=US&key={3}", city, state, postalcode, key);

                string response = new WebClient().DownloadString(uri);

                RootObject dser = JsonConvert.DeserializeObject<RootObject>(response);
                Dictionary<string, double> coords = getLatLong(dser);
                return getDbGeography(coords["lat"], coords["lng"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbGeography GetFromStateZip(string state, string postalcode)
        {
            string key = ConfigurationManager.AppSettings["bingMapsKey"].ToString();

            // http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?includeNeighborhood=includeNeighborhood&maxResults=maxResults&key=BingMapsKey
            // http://msdn.microsoft.com/en-us/library/ff701714.aspx

            try
            {
                string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/?adminDistrict={0}&postalCode={1}&countryRegion=US&key={2}", state, postalcode, key);

                string response = new WebClient().DownloadString(uri);

                RootObject dser = JsonConvert.DeserializeObject<RootObject>(response);
                Dictionary<string, double> coords = getLatLong(dser);
                return getDbGeography(coords["lat"], coords["lng"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DbGeography getDbGeography(double lat, double lng)
        {
            return DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));
        }

        private Dictionary<string, double> getLatLong(RootObject rootObject)
        {
            if (rootObject.statusCode == 200 && rootObject.resourceSets != null && rootObject.resourceSets.Count == 1)
            {
                var lat = rootObject.resourceSets[0].resources[0].point.coordinates[0];
                var lng = rootObject.resourceSets[0].resources[0].point.coordinates[1];
                return new Dictionary<string, double> { { "lat", lat }, { "lng", lng } };
            }
            else
            {
                return null;
            }
        }
    }
}