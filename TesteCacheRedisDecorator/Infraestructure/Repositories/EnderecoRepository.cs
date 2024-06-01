using Microsoft.EntityFrameworkCore;
using TesteCacheRedisDecorator.Contexts;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;
using TesteCacheRedisDecorator.Models;

namespace TesteCacheRedisDecorator.Infraestructure.Repositories;

public class EnderecoRepository(AppDbContext db, ILogger<EnderecoRepository> logger) : IRepository<Endereco>
{
    public async Task<List<Endereco>> GetAllAsync(int take, int skip)
    {
        try
        {
            return await db.Enderecos.AsNoTracking().Skip(skip).Take(take).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<Endereco?> GetAsync(int id)
    {
        try
        {
            return await db.Enderecos.FindAsync(id);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<Endereco> AddAsync(Endereco item)
    {
        try
        {
            db.Enderecos.Add(item);
            await db.SaveChangesAsync();
            return item;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task EditAsync(Endereco item)
    {
        try
        {
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();

        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var endereco = await db.Enderecos.FindAsync(id);
            if (endereco != null)
            {
                db.Enderecos.Remove(endereco);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}