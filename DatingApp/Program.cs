using DatingApp.Data;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext> (opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors ( );
builder.Services.AddScoped<ITokenService, TokenService> ( );
builder.Services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer (options =>
    {
        var tokenkey = builder.Configuration["TokenKey"] 
                       ?? throw new InvalidOperationException("TokenKey is not founded -- program.cs");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors (x => x.AllowAnyHeader ( ).AllowAnyMethod ( ).WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication ( );
app.UseAuthorization ( );

app.MapControllers();
app.Run();
