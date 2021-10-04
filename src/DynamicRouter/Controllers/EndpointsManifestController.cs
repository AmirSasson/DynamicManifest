using DynamicRoutes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRoutes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EndpointsManifestController : ControllerBase
    {
        public static Dictionary<string, ApiEndpoint> ManifestDB = new Dictionary<string, ApiEndpoint>();

        private readonly ILogger<EndpointsManifestController> _logger;

        public EndpointsManifestController(ILogger<EndpointsManifestController> logger)
        {
            _logger = logger;
        }

        [HttpPut]
        public IActionResult Put(ApiEndpoint endpoint)
        {
            ManifestDB[endpoint.Path.ToClean()] = endpoint;
            return Ok();
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(ManifestDB);
        }

        //[HttpGet("external")]
        //public IActionResult GetExternal()
        //{
        //    return Ok(ManifestDB);
        //}
    }
}
