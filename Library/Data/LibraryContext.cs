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
        public DbSet<Library.Models.Item> Item { get; set; } = default!;

        public DbSet<Library.Models.Borrower>? Borrower { get; set; }

        public DbSet<Library.Models.BorrowingHistory>? BorrowingHistory { get; set; }

        public DbSet<Library.Models.BorrowingDetail>? BorrowingDetail { get; set; }

    }
}
