using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using System;
using Azure;

namespace Moln.KeyVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private SecretClient _client;
        public string _secret;
        IConfiguration _configuration;
        public WeatherForecastController()
        {


        }


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> GetAsync()
        {
            
            string keyVaultUrl = "https://bobo.vault.azure.net/";
            string secretName = "Gustav";

            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

            try
            {
                KeyVaultSecret secret =  client.GetSecret(secretName);

                Console.WriteLine($"Retrieved secret '{secret.Name}': {secret.Value}");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error retrieving secret: {ex.Message}");
            }

            return _secret;
        }
    }
}