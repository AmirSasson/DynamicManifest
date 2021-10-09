using DynamicRoutes.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DynamicRoutes.DataAccess
{
    public interface IEndpointsManifestRespository
    {
        Task<ApiEndpoint> Add(ApiEndpoint endpoint);
        Task<IEnumerable<ApiEndpoint>> GetAll();
    }
}
