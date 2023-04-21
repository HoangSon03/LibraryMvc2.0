using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class HistoryRepository : IGenericRepository<BorrowingHistory>

    {
        private readonly LibraryContext _context;
        private DbSet<BorrowingHistory> _dbset;

        public HistoryRepository(LibraryContext context)
        {
            _context = context;
            _dbset = context.Set<BorrowingHistory>();
        }

        public async Task<BorrowingHistory> Get(int id)
        {
            return await _dbset.Include(y=>y.BorrowingDetails).Include(x => x.Borrower).SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<BorrowingHistory> GetAll()
        {
            return _dbset.Include(x => x.Borrower).AsEnumerable().OrderByDescending(x => x.Id);
        }

        public async Task Create(BorrowingHistory entity)
        {

            _dbset.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(BorrowingHistory entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(BorrowingHistory entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<BorrowingHistory> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMany(IEnumerable<BorrowingHistory> entity)
        {
            throw new NotImplementedException();
        }
    }
}
