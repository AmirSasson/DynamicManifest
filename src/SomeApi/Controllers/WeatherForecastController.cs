using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ManifestEndpoint(true, 50)]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        [ManifestEndpoint]
        public WeatherForecast GetById(int id)
        {
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 4,
                Summary = WeatherForecastController.Summaries[0]
            };
        }

        [HttpGet("Laith")]
        [ManifestEndpoint(true, 6000)]
        public WeatherForecast GetById2(int id)
        {
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 4,
                Summary = WeatherForecastController.Summaries[0]
            };
        }
    }


    public class ManifestEndpointAttribute : Attribute
    {
        public bool Register { get; set; } = true;
        public int ThrottleLimit { get; set; } = 2500;
        public string SomeMore{ get; set; }
        public ManifestEndpointAttribute(bool register, int throttleLimit)
        {
            Register = register;
            ThrottleLimit = throttleLimit;
        }

        public ManifestEndpointAttribute()
        {                       
        }
    }
}


