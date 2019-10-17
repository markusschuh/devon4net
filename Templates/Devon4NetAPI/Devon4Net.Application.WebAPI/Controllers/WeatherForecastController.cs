using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Devon4Net.Infrastructure.Common.Options.Devon;
using Devon4Net.Infrastructure.Common.Options.Swagger;
using Devon4Net.Infrastructure.JWT.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Devon4Net.Application.WebAPI.Controllers
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
        private readonly SwaggerOptions _swaggerOptions;
        private readonly DevonfwOptions _devonfwOptions;
        private readonly IJwtHandler _jwtHandler;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<SwaggerOptions> swaggerOptions,
            IOptions<DevonfwOptions> devonfwOptions, IJwtHandler jwtHandler)
        {
            _logger = logger;
            _swaggerOptions = swaggerOptions.Value;
            _devonfwOptions = devonfwOptions.Value;
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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

        /// <summary>
        /// Gets a sample of jwt
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/jwt")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [ProducesResponseType(500)]
        public string GetJwtToken()
        {
            return _jwtHandler.CreateClientToken(new List<Claim>());
        }
    }
}