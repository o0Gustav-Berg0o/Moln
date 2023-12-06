using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moln.A7.Data;
using Moln.A7.Models;

namespace Moln.A7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {     
        public IConfiguration _config;
        public SqlContext _context;

        public WeatherForecastController(IConfiguration config, SqlContext context)
        {
            _context = context;
            _config = config;            
        }

        [HttpGet]
        public string Get()
        {
            if (_context.Users.Count() < 2)
            {
                var UserEntity = new User
                {
                    Names = "Bob"
                };


                var UserEntity2 = new User
                {
                    Names = "Penny"
                };

               _context.Users.Add(UserEntity);
               _context.Users.Add(UserEntity2);
               _context.SaveChanges();
                return "Skapat 2 users";
            }
            else
            {
                return "Get kördes men inga users skapade";
            }
            
        }
    }
}
