using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class DetailRepository : IGenericRepository<BorrowingDetail>

    {
        private readonly LibraryContext _context;
        private DbSet<BorrowingDetail> _dbset;

        public DetailRepository(LibraryContext context)
        {
            _context = context;
            _dbset = context.Set<BorrowingDetail>();
        }

        public async Task<BorrowingDetail> Get(int id)
        {
            return await _dbset.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<BorrowingDetail> GetAll()
        {
            return _dbset.AsEnumerable().OrderByDescending(x => x.Id);
        }

        public async Task Create(BorrowingDetail entity)
        {

            _dbset.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(BorrowingDetail entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMany(IEnumerable<BorrowingDetail> entity)
        {
            _dbset.RemoveRange(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(BorrowingDetail entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }
      
        public IEnumerable<BorrowingDetail> GetAllById(int id)
        {
            return _dbset.Where(x=>x.BorrowingHistoryId == id ).Include(y=>y.Item).AsEnumerable().OrderByDescending(x => x.Id);
        }

       
    }
}
