using Autofac;
using Autofac.Extensions.DependencyInjection;
using UserApi.DB;
using Microsoft.EntityFrameworkCore;
using DbApi.Mapper;


var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        // Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile));


builder.Services.AddEndpointsApiExplorer();
var config = new ConfigurationBuilder();
config.AddJsonFile("appsettings.json");
var cfg = config.Build();

builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
{
     cb.Register(c => new AppDbContext(builder.Configuration.GetConnectionString("db"))).InstancePerDependency();
});        



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
    