using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeApi
{
    public interface IManifestProvider
    {
        Task Register(IEnumerable<EndpointDataSource> endpointSources);
    }
}