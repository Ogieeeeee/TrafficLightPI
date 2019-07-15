using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrafficlightAPI.Models
{
    public static class Call
    {
        public static async Task<int> Get(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                Pulse pulse = JsonConvert.DeserializeObject<Pulse>(response.Content.ReadAsStringAsync().Result);
                return pulse.pulse;

                // response.Content.ReadAsStringAsync()
                // System.Console.WriteLine(response.Result);
            }
        }
    }
}
