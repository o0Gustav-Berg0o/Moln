using Microsoft.AspNetCore.Mvc;

namespace Configuration_Demo.Controllers
{
    [ApiController]
    [Route("config")]
    public class WeatherForecastController : ControllerBase
    {
       

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            string appSetting = _configuration["AppSettings:Title"];            
            string currentEnvironment = _configuration["Environment"];
            return Ok($"Current environment : {currentEnvironment} \nAppsetting : {appSetting}");
        }
    }
}
