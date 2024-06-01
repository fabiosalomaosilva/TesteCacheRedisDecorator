using Microsoft.EntityFrameworkCore;
using TesteCacheRedisDecorator.Contexts;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;
using TesteCacheRedisDecorator.Models;

namespace TesteCacheRedisDecorator.Infraestructure.Repositories
{
    public class PessoaRepository(AppDbContext db, ILogger<PessoaRepository> logger) : IRepository<Pessoa>
    {
        public async Task<List<Pessoa>> GetAllAsync(int take, int skip)
        {
            try
            {
                return await db.Pessoas.AsNoTracking().Skip(skip).Take(take).ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<Pessoa?> GetAsync(int id)
        {
            try
            {
                return await db.Pessoas.FindAsync(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<Pessoa> AddAsync(Pessoa item)
        {
            try
            {
                db.Pessoas.Add(item);
                await db.SaveChangesAsync();
                return item;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        public async Task EditAsync(Pessoa item)
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
                var pessoa = await db.Pessoas.FindAsync(id);
                if (pessoa != null)
                {
                    db.Pessoas.Remove(pessoa);
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
}
