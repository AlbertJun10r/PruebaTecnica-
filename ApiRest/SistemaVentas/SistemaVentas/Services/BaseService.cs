using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Services
{
    public class BaseService<T> : IService<T> where T : class
    {
        private readonly PruebaTecnicaContext _context;

        public BaseService(PruebaTecnicaContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) 
            { 
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
