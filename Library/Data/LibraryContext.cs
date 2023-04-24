using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext (DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Item { get; set; } = default!;

        public DbSet<Borrower> Borrower { get; set; }

        public DbSet<BorrowingHistory> BorrowingHistory { get; set; }

        public DbSet<BorrowingDetail> BorrowingDetail { get; set; }

    }
}
