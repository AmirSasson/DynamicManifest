using System.Collections.Generic;

namespace DynamicRoutes.Controllers
{
    public class ApiEndpoint
    {
        public string Path { get; set; }
        public int Port { get; set; }
        public int ThrottleLimit { get; set; } = 2500;
        public IEnumerable<string> ApiVersions { get; set; }

        // whatever...metadata ...
    }
}
