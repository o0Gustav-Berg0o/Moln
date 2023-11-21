using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Moln.WebApi.ConnectDemo.Data
{
    public class Seed
    {
        public static void SeedData(SqlContext context)
        {
            if (context.Users.Count() < 2)
            {
                var UserEntity = new User
                {
                    Names = "Bob"
                };
              

                var UserEntity2 = new User
                {
                    Names = "Penny"
                };          

                context.Users.Add(UserEntity);  
                context.Users.Add(UserEntity2);
                context.SaveChanges();
            }
        }
    }
}