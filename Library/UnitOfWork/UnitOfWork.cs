using Library.Data;
using Library.Models;
using Library.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;
        public ItemRepository Items { get; private set; }
        public GenericRepository<Borrower> Borrowers { get; private set; }
        public HistoryRepository BorrowingHistories { get; private set; }
        public DetailRepository BorrowingDetails { get; private set; }

        public UnitOfWork(LibraryContext context)
        {
            _context = context;
            Items = new ItemRepository(context);
            BorrowingDetails = new DetailRepository(context);
            BorrowingHistories = new HistoryRepository(context);
            Borrowers = new GenericRepository<Borrower>(context);
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
