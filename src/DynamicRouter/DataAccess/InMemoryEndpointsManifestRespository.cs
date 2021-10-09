using DynamicRoutes.Controllers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace DynamicRoutes.DataAccess
{
    public class InMemoryEndpointsManifestRespository : BaseEndpointsManifestRespository
    {
        private readonly ConcurrentDictionary<string, ApiEndpoint> _manifestDB = new ConcurrentDictionary<string, ApiEndpoint>();

        public override Task<ApiEndpoint> Add(ApiEndpoint endpoint)
        {
            var recordId = $"{endpoint.Service}#{endpoint.Path.ToClean()}#{endpoint.Port}";
            _manifestDB[recordId] = endpoint;
            return endpoint.AsTask();
        }

        public override Task<IEnumerable<ApiEndpoint>> GetAll()
        {
            return _manifestDB.Values
                .GroupBy(ep => ep.Path.ToClean())
                .Select(grp => getMaxPriorityInGroup(grp))
                .AsEnumerable().AsTask();
        }
    }
}
