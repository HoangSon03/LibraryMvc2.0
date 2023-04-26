using Library.Models;

namespace Library.ViewModel
{
    public class DetailViewModel
    {
        public BorrowingHistory BorrowingHistory { get; set; }
        public ICollection<BorrowingDetail> BorrowingDetail { get; set; }
        public Borrower Borrower { get; set; }

        public int DetailId { get; set; }
        public int ItemId { get; set; }
        public DateTime? ReturnDate { get; set; }
        //public DateTime? BorrowDate { get; set; }

    }
}
