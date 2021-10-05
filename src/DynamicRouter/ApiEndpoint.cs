namespace DynamicRoutes.Controllers
{
    public class ApiEndpoint
    {
        public string Path { get; set; }
        public int Port { get; set; }
        public int ThrottleLimit { get; set; } = 2500;

        // whatever...metadata ...
    }
}
