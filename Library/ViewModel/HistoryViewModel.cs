using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.ViewModel
{
    public class HistoryViewModel
    {
        public List<Item>? ListItem { get; set; }

        [Required]
        public List<int> ListSelectItem { get; set; }
        public List<int> ListSelectQuantity { get; set; }
        public int BorrowerId { get; set; }
    }
}
