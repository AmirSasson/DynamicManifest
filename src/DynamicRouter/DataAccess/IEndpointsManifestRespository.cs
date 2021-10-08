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
        private readonly ConcurrentDictionary<string, ApiEndpoint> _manifestDB = new ConcurrentDictionary<string, ApiEndpoint>();

        public Task<ApiEndpoint> Add(ApiEndpoint endpoint)
        {
            _manifestDB.TryGetValue(endpoint.Path.ToClean(), out var existingResgistration);

            if (existingResgistration != null && existingResgistration.EndpointsPriority > endpoint.EndpointsPriority)
            {
                // Illegal registration a higher version already registered
                return endpoint.AsTask();
            }
            else
            {
                _manifestDB[endpoint.Path.ToClean()] = endpoint;
            }


            return endpoint.AsTask();
        }

        public Task<IEnumerable<ApiEndpoint>> GetAll()
        {
            return _manifestDB.Values.AsEnumerable().AsTask();
        }
    }
}
