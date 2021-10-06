using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface IManifestProvider
    {
        Task Register(IEnumerable<EndpointDataSource> endpointSources, ServerAddress serverAddress);
    }
    public class ServerAddress
    {
        public int Port { get; set; }
    }
}