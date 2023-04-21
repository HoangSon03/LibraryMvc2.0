using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class ItemRepository : IGenericRepository<Item>

    {
        private readonly LibraryContext _context;
        private DbSet<Item> _dbset;

        public ItemRepository(LibraryContext context)
        {
            _context = context;
            _dbset = context.Set<Item>();
        }

        public async Task<Item> Get(int id)
        {
            return await _dbset.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Item> GetAll()
        {
            return _dbset.Where(x=>x.Quantity>0).AsEnumerable().OrderByDescending(x => x.Id);
        }

        public async Task Create(Item entity)
        {

            _dbset.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Item entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Item entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Item> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMany(IEnumerable<Item> entity)
        {
            throw new NotImplementedException();
        }
    }
}
