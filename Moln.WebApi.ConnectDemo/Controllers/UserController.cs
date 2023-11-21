using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moln.WebApi.ConnectDemo.Data;

namespace Moln.WebApi.ConnectDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
     
        private readonly ILogger<UserController> _logger;
        private SqlContext _context;

        public UserController(ILogger<UserController> logger, SqlContext context)
        {
            _context = context;
            _logger = logger;
        }   

        [HttpGet(Name = "Users")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var result = await _context.Users.ToListAsync();
            return Ok(result);
        }
    }
}
