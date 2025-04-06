using Microsoft.EntityFrameworkCore;
using ServerPDV;
using ServerPDV.Interfaces;

//..
namespace Servidor_Frota.API.Repositories
{
    public abstract class CrudRepository<T> : ICrud<T> where T : class
    {

        protected readonly CaixaSupermercadoContext _context;

        public CrudRepository(CaixaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Salvar(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                await _context.AddAsync(entity);
            }
            else
            {
                _context.Update(entity);
            }

            await _context.SaveChangesAsync();

            return await _context.Set<T>().FindAsync(_context.Entry(entity).Property("Id").CurrentValue);
        
        }
        public async Task<T> Update(T entity)
        {

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Selecionar(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> SelecionarTodos()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ExecutarQuery(string query)
        {
            return await _context.Set<T>().FromSqlRaw(query).ToListAsync();
        }
    }
}