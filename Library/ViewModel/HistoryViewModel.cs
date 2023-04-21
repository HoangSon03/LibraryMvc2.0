using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.ViewModel
{
    public class HistoryViewModel
    {
        public List<int>? ListItemId { get; set; }
        public int BorrowerId { get; set; }
    }
}
