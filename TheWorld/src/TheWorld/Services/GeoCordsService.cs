using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class GeoCordsService : IGeoCordsService
    {
        ILogger<GeoCordsService> Logger;
        IConfigurationRoot Config;
        public GeoCordsService(ILogger<GeoCordsService> logger, IConfigurationRoot config)
        {
            Logger = logger;
            Config = config;
        }

        public async Task<GeoCordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCordsResult()
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = Config["GeoCordService:APIKey"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = Config["GeoCordService:serviceUrl"] + $"q={encodedName}" + $"&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;
        }
    }
}

