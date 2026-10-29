using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationDemo.Controllers;

// Använd Options Pattern
public class MySettings
{
    public string ApiKey { get; set; }
    public string DatabaseConnection { get; set; }
    public bool FeatureEnabled { get; set; }
    public int MaxRetries { get; set; }
}

[ApiController]
[Route("api/options")]
public class MyController : ControllerBase
{
    private readonly MySettings _settings;

    public MyController(IOptions<MySettings> settings)
    {
        _settings = settings.Value;
    }

    [HttpGet]
    public IActionResult Get() => Ok(_settings);
}
