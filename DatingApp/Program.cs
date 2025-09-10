using DatingApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext> (opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors ( );
var app = builder.Build();

app.UseCors (x => x.AllowAnyHeader ( ).AllowAnyMethod ( ).WithOrigins("http://localhost:4200", "https://localhost:4200"));
// Configure the HTTP request pipeline.
app.MapControllers();
app.Run();
