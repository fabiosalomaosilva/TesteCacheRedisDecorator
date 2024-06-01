using Microsoft.EntityFrameworkCore;
using TesteCacheRedisDecorator.Contexts;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;
using TesteCacheRedisDecorator.Infraestructure.Repositories;
using TesteCacheRedisDecorator.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services
builder.Services.AddTransient<RedisContext>();
builder.Services.AddTransient<IRepository<Pessoa>, PessoaRepository>();
builder.Services.AddTransient<IRepository<Endereco>, EnderecoRepository>();
builder.Services.Decorate(typeof(IRepository<>), typeof(CachedRepository<>));


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
