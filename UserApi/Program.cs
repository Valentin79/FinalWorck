using Autofac;
using Autofac.Extensions.DependencyInjection;
using DbApi.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using UserApi;
using UserApi.Abstractions;
using UserApi.DB;
using UserApi.Service;




static RSA GetPublicKey()
{
    var f = File.ReadAllText("rsa/public_key.pem");
    var rsa = RSA.Create();
    rsa.ImportFromPem(f);
    return rsa;
}

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        // Add services to the container.

builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddSwaggerGen(opt =>
{
     opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
         In = ParameterLocation.Header,
         Description = "Please enter token",
         Name = "Authorization",
         Type = SecuritySchemeType.Http,
         BearerFormat = "Token",
         Scheme = "bearer"
     });
     opt.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
            {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                },
            new string[] {}
            }
     });
    });

var config = new ConfigurationBuilder();
config.AddJsonFile("appsettings.json");
var cfg = config.Build();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.Register(c => new AppDbContext(cfg.GetConnectionString("db"))).InstancePerDependency();
    });


builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserAuthenticationService, AuthenticationMock>(); // только для тестирования



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new RsaSecurityKey(GetPublicKey())

    };
}
);

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
    

