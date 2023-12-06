using Microsoft.EntityFrameworkCore;
using Moln.A7.Data;

namespace Moln.A7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            ///Lägg till
            string connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<SqlContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString));
            ///

            var app = builder.Build();

         
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();              
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
