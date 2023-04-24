using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Repositories
{
    public class ItemRepository : GenericRepository<Item>

    {
        private readonly LibraryContext _context;
        private DbSet<Item> _dbset;

        public ItemRepository(LibraryContext context) : base(context)
        {
            _context = context;
            _dbset = context.Set<Item>();
        }
        public override IEnumerable<Item> GetAll()
        {
            return _dbset.Where(x=>x.Quantity>0).AsEnumerable().OrderByDescending(x => x.Id);
        }
    }
}
