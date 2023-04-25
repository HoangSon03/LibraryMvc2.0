using Library.Models;
using Library.Repositories;

namespace Library.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ItemRepository Items { get; }
        GenericRepository<Borrower> Borrowers { get; }
        HistoryRepository BorrowingHistories { get; }
        DetailRepository BorrowingDetails { get; }
        Task Save();
    }
}
