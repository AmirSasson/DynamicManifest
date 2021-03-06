using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IManifestProvider
    {
        Task Register(IEnumerable<EndpointDataSource> endpointSources, ServerAddress serverAddress, int defaultEndpointsPriority);
    }
    public class ServerAddress
    {
        public int Port { get; set; }
        public string ServerName { get; set; }
    }
}