using Moln.A7.Data;
using Moln.A7.Models;

namespace Moln.A7.Data
{
    public class Seed
    {        public static void SeedData(SqlContext context)
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