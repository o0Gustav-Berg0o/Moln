using Microsoft.EntityFrameworkCore;
using SlutProv.Api.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabase")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Scalar är påslaget även i molnet bara för övningen. Stäng av det i skarpa appar.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
