using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.ViewModel
{
    public class HistoryViewModel
    {
        public List<Item>? ListItem { get; set; }
        public List<int>? ListSelectItem { get; set; }

        public int BorrowerId { get; set; }
        public int BorrowQuantity { get; set; }
    }
}
