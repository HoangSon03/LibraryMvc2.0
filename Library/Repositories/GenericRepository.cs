using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LibraryContext _context;
        private DbSet<T> _dbset;

        public GenericRepository(LibraryContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public async Task<T> Get(int id)
        {
            return await _dbset.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable().OrderByDescending(x => x.Id);
        }

        public async Task Create(T entity)
        {

            _dbset.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMany(IEnumerable<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}
