using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moln.AppInsight.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FelhanteringsController : ControllerBase
    {

        private readonly ILogger<FelhanteringsController> _logger;

        public FelhanteringsController(ILogger<FelhanteringsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]        
        public string Get()
        {
            _logger.LogInformation("Vi loggar information");
            _logger.LogError("Vi loggar ett fel");
            _logger.LogCritical("Vi loggar kritiskt fel");
            return "Vi har loggat lite fel";
        }
    }
}
