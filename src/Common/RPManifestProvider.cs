using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common
{
    public class RPManifestProvider : IManifestProvider
    {
        public async Task Register(IEnumerable<EndpointDataSource> endpointSources, ServerAddress serverAddress, int defaultEndpointsPriority)
        {
            HttpClient c = new HttpClient();
            var url = "http://localhost:5000/EndpointsManifest";

            foreach (var endpoint in (endpointSources.First().Endpoints.Cast<RouteEndpoint>()))
            {
                var manifestEndpointMetaData = endpoint?.Metadata?.SingleOrDefault(md => md is ManifestEndpointAttribute) as ManifestEndpointAttribute;
                if (manifestEndpointMetaData?.Register ?? false)
                {
                    HashSet<string> versions = getRouteVersion(endpoint);

                    try
                    {
                        var resp = await c.PutAsJsonAsync(url, new { endpointsPriority = manifestEndpointMetaData.EndpointsPriority ?? defaultEndpointsPriority, throttleLimit = manifestEndpointMetaData.ThrottleLimit, path = endpoint.RoutePattern.RawText, port = serverAddress.Port, apiVersions = versions });
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

            }
        }

        private static HashSet<string> getRouteVersion(RouteEndpoint endpoint)
        {
            var versions = new HashSet<string>();
            var apiVersionAction = endpoint?.Metadata?.SingleOrDefault(md => md is MapToApiVersionAttribute) as MapToApiVersionAttribute;
            if (apiVersionAction == null)
            {
                var apiVersionsList = endpoint?.Metadata?.Where(md => md is ApiVersionAttribute).Cast<ApiVersionAttribute>();

                foreach (var apiVersionAtt in apiVersionsList)
                {
                    foreach (var ver in apiVersionAtt.Versions)
                    {
                        versions.Add(ver.ToString());
                    }
                }
            }

            return versions;
        }
    }

}