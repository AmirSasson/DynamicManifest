using DynamicRoutes;
using DynamicRoutes.DataAccess;
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
        private readonly ILogger<EndpointsManifestController> _logger;
        private readonly IEndpointsManifestRespository _manifestRepo;

        public EndpointsManifestController(ILogger<EndpointsManifestController> logger, IEndpointsManifestRespository manifestRepo)
        {
            _logger = logger;
            _manifestRepo = manifestRepo;
        }

        [HttpPut]
        public async Task<IActionResult> Put(ApiEndpoint endpoint)
        {
            await _manifestRepo.Add(endpoint);
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manifestRepo.GetAll());
        }

        //[HttpGet("external")]
        //public IActionResult GetExternal()
        //{
        //    return Ok(ManifestDB);
        //}
    }
}
