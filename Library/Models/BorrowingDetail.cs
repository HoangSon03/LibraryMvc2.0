using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Library.Models
{
    public class BorrowingDetail:BaseEntity
    {
        [ForeignKey("BorrowingHistory")]
        public int BorrowingHistoryId { get; set; }
        public virtual BorrowingHistory BorrowingHistory { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Return Date")]
        [AllowNull]
        public DateTime? ReturnDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Cost { get; set; }

    }
}
