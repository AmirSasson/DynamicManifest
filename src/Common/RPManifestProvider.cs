using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common
{
    public class RPManifestProvider : IManifestProvider
    {
        private readonly ILogger<RPManifestProvider> _logger;

        public RPManifestProvider(ILogger<RPManifestProvider> logger)
        {
            _logger = logger;
        }
        public async Task Register(IEnumerable<EndpointDataSource> endpointSources, ServerAddress serverAddress, int defaultEndpointsPriority)
        {
            HttpClient c = new HttpClient();
            var url = "http://localhost:8080/EndpointsManifest";

            foreach (var endpoint in (endpointSources.First().Endpoints.Cast<RouteEndpoint>()))
            {
                var manifestEndpointMetaData = endpoint?.Metadata?.SingleOrDefault(md => md is ManifestEndpointAttribute) as ManifestEndpointAttribute;
                if (manifestEndpointMetaData?.Register ?? false)
                {
                    HashSet<string> versions = getRouteVersion(endpoint);

                    var ep = new { service = serverAddress.ServerName, endpointsPriority = manifestEndpointMetaData.EndpointsPriority ?? defaultEndpointsPriority, throttleLimit = manifestEndpointMetaData.ThrottleLimit, path = endpoint.RoutePattern.RawText, port = serverAddress.Port, apiVersions = versions };
                    try
                    {

                        var resp = await c.PutAsJsonAsync(url,ep );
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"failed to register {ep.ToJson()}");
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
