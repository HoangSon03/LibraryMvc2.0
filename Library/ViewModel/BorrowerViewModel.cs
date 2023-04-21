using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.ViewModel
{
    public class BorrowerViewModel
    {
        public List<Borrower>? Borrowers { get; set; }
        public string? BorrowerSearch { get; set; }
    }
}
