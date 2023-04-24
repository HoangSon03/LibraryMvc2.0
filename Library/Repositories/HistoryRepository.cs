using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class HistoryRepository : GenericRepository<BorrowingHistory>

    {
        private readonly LibraryContext _context;
        private DbSet<BorrowingHistory> _dbset;

        public HistoryRepository(LibraryContext context):base (context)
        {
            _context = context;
            _dbset = context.Set<BorrowingHistory>();
        }

        public override async Task<BorrowingHistory> Get(int id)
        {
            return await _dbset.Include(y=>y.BorrowingDetails).Include(x => x.Borrower).SingleOrDefaultAsync(x => x.Id == id);
        }

        public override IEnumerable<BorrowingHistory> GetAll()
        {
            return _dbset.Include(x => x.Borrower).AsEnumerable().OrderByDescending(x => x.Id);
        }
    }
}
