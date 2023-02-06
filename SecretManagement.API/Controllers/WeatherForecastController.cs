using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SecretManagement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IOptionsMonitor<DatabaseSettings> _databaseSettings;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IOptionsMonitor<DatabaseSettings> databaseSettings)
    {
        _logger = logger;
        _databaseSettings = databaseSettings;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        return Ok(_databaseSettings.CurrentValue.ConnectionString);
    }
}