using Microsoft.EntityFrameworkCore;
using Moln.A7.Models;

namespace Moln.A7.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
