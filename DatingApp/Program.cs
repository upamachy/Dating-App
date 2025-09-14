using DatingApp.Data;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext> (opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors ( );
builder.Services.AddScoped<ITokenService, TokenService> ( );
var app = builder.Build();

app.UseCors (x => x.AllowAnyHeader ( ).AllowAnyMethod ( ).WithOrigins("http://localhost:4200", "https://localhost:4200"));
// Configure the HTTP request pipeline.
app.MapControllers();
app.Run();
