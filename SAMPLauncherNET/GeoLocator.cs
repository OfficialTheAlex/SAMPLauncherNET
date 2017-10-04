﻿using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// SA:MP launcher .NET namespace
/// </summary>
namespace SAMPLauncherNET
{
    /// <summary>
    /// Geo locator class
    /// </summary>
    public static class GeoLocator
    {
        /// <summary>
        /// Cache
        /// </summary>
        private static Dictionary<string, GeoLocationData> cache = new Dictionary<string, GeoLocationData>();

        /// <summary>
        /// Providers
        /// </summary>
        private static readonly GeoLocationProvider[] providers = new GeoLocationProvider[] { new GeoLocationProvider("http://freegeoip.net/json/", typeof(FreeGeoIPDataContract)) };

        /// <summary>
        /// Locate
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>Geo location data</returns>
        public static GeoLocationData Locate(string host)
        {
            GeoLocationData ret = null;
            host = host.Trim();
            if (cache.ContainsKey(host))
                ret = cache[host];
            else
            {
                foreach (GeoLocationProvider provider in providers)
                {
                    ret = provider.RequestData(host);
                    if (ret != null)
                        break;
                }
            }
            if (ret == null)
                ret = new GeoLocationData(null);
            return ret;
        }

        /// <summary>
        /// Locate asynchronous
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>Geo location data</returns>
        public static Task<GeoLocationData> LocateAsync(string host)
        {
            return Task.Factory.StartNew(() => Locate(host));
        }
    }
}
