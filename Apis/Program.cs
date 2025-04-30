using Application.Interfaces;
using Application.Services;
using Data.Contexts;
using Data.InternalServices;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            NameClaimType = "Name"
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var sessionGuidClaim = context.Principal?.FindFirst("session_guid")?.Value;
                if (string.IsNullOrEmpty(sessionGuidClaim))
                {
                    context.Fail("Invalid session GUID.");
                    return;
                }

                using var scope = context.HttpContext.RequestServices.CreateScope();
                var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
                var isValid = await jwtService.ValidateSessionAsync(sessionGuidClaim);

                if (!isValid)
                {
                    context.Fail("Session is invalid or expired.");
                }
            }
        };
    });

// Conexión 1: Base de datos principal
builder.Services.AddDbContext<BumpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JavierConnection")));



// Registrar servicios y repositorios
builder.Services.AddScoped<ISesionService, SesionService>();
builder.Services.AddScoped<ISesionRepository, SesionRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
