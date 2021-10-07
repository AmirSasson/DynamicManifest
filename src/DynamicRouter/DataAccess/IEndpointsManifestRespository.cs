using DynamicRoutes.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace DynamicRoutes.DataAccess
{
    public interface IEndpointsManifestRespository
    {
        Task<ApiEndpoint> Add(ApiEndpoint endpoint);
        Task<IEnumerable<ApiEndpoint>> GetAll();
    }

    public class InMemoryEndpointsManifestRespository : IEndpointsManifestRespository
    {
        private readonly ConcurrentBag<ApiEndpoint> _manifestDB = new ConcurrentBag<ApiEndpoint>();

        public Task<ApiEndpoint> Add(ApiEndpoint endpoint)
        {
            if(_manifestDB.Any(e => e.Path == endpoint.Path.ToClean() && endpoint.EndpointsPriority < e.EndpointsPriority))
            {
                // Illegal registrtion 
            }
            else
            {
                _manifestDB.Add(endpoint);
            }                       
            return endpoint.AsTask();
        }

        public Task<IEnumerable<ApiEndpoint>> GetAll()
        {
            return _manifestDB.AsEnumerable().AsTask();
        }
    }
}
