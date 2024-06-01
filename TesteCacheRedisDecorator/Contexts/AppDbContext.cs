using Microsoft.EntityFrameworkCore;
using TesteCacheRedisDecorator.Models;

namespace TesteCacheRedisDecorator.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
