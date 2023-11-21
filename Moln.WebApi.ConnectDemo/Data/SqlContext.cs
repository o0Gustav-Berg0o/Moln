using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Moln.WebApi.ConnectDemo.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions options) : base(options)        {
            
        }
       public DbSet<User> Users { get; set; }
    }
}
