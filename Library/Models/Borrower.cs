using Microsoft.Build.Framework;

namespace Library.Models
{
    public class Borrower:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
    }
}
