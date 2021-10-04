using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomeApi
{
    public class RPManifestProvider : IManifestProvider
    {
        public async Task Register(IEnumerable<EndpointDataSource> endpointSources)
        {
            HttpClient c = new HttpClient();
            var url = "http://localhost:5000/EndpointsManifest";

            foreach (var endpoint in (endpointSources.First().Endpoints.Cast<RouteEndpoint>()))
            {
                try
                {
                    var resp = await c.PutAsJsonAsync(url, new { path = endpoint.RoutePattern.RawText, port = 5001 });
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }

}