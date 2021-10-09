using DynamicRoutes.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRoutes.DataAccess
{
    public abstract class BaseEndpointsManifestRespository : IEndpointsManifestRespository
    {
        public abstract Task<ApiEndpoint> Add(ApiEndpoint endpoint);

        public abstract Task<IEnumerable<ApiEndpoint>> GetAll();

        protected ApiEndpoint getMaxPriorityInGroup(IGrouping<string, ApiEndpoint> grp)
        {
            var max = grp.First();
            foreach (ApiEndpoint ep in grp)
            {
                if (ep.EndpointsPriority > max.EndpointsPriority)
                {
                    max = ep;
                }
            }
            return max;
        }
    }
}
