using Microsoft.AspNetCore.Mvc;
using WeatherAPIDeploy;

namespace WeatherAPIDeploy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "PRODUCTION", "PRODUCTION", "PRODUCTION", "PRODUCTION", "PRODUCTION",
            "PRODUCTION", "PRODUCTION", "PRODUCTION", "PRODUCTION", "PRODUCTION"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // Extra endpoint för att se slot info
        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            var slotName = Environment.GetEnvironmentVariable("WEBSITE_SLOT_NAME") ?? "production";
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";

            return Ok(new
            {
                Version = "1.0.0",
                Branch = "main",
                Slot = slotName,
                Environment = environment,
                Timestamp = DateTime.UtcNow,
                Message = " PRODUCTION VERSION"
            });
        }
    }
}
