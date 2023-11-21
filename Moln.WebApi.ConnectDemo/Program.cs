
using Microsoft.EntityFrameworkCore;
using Moln.WebApi.ConnectDemo.Data;

namespace Moln.WebApi.ConnectDemo
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
            builder.Services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), providerOptions => providerOptions.EnableRetryOnFailure());
                options.EnableSensitiveDataLogging(true);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<SqlContext>();
                    Seed.SeedData(dataContext);
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
