using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class DetailRepository : GenericRepository<BorrowingDetail>

    {
        private readonly LibraryContext _context;
        private DbSet<BorrowingDetail> _dbset;

        public DetailRepository(LibraryContext context):base(context) 
        {
            _context = context;
            _dbset = context.Set<BorrowingDetail>();
        }

        public  async Task DeleteMany(IEnumerable<BorrowingDetail> entity)
        {
            _dbset.RemoveRange(entity);
        }

        public  IEnumerable<BorrowingDetail> GetAllById(int id)
        {
            return _dbset.Where(x=>x.BorrowingHistoryId == id ).Include(y=>y.Item).AsEnumerable().OrderByDescending(x => x.Id);
        }

       
    }
}
