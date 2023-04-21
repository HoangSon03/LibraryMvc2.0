using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Library.Models
{
    public class BorrowingHistory : BaseEntity
    {
        [ForeignKey("Borrower")]
        public int BorrowerId { get; set; }
        public virtual Borrower Borrower { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Borrow Date")]
        public DateTime BorrowDate { get; set; }

        [AllowNull]
        [DisplayName("Total Items")]
        public int? CountItem { get; set; }

        [AllowNull]
        [DisplayName("Total Quantity")]
        public int? CountItemQuantity { get; set; }

        [AllowNull]
        [DisplayName("Total Cost")]
        public decimal? TotalCost { get; set; }

        public string? Status { get; set; }

        public ICollection<BorrowingDetail> BorrowingDetails { get; set; }
    }
}
