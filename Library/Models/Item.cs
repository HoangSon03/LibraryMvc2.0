using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Item:BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }

        [DisplayName("Run Time")]
        public string? RunTime { get; set; }

        [DisplayName("Num Of Page")]
        public int? NumOfPage { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Category Category { get; set; }

        public ICollection<BorrowingDetail> BorrowingDetails { get; set; }
    }
}
