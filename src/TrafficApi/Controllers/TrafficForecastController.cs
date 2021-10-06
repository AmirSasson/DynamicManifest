using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Very Busy", "Moderate", "Free"
        };

        private readonly ILogger<TrafficForecastController> _logger;

        public TrafficForecastController(ILogger<TrafficForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ManifestEndpoint]
        public IEnumerable<TrafficForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new TrafficForecast
            {
                Date = DateTime.Now.AddDays(index),
                BusyScore = rng.Next(-20, 55),
                RoadId = index,
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
