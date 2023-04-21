using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.ViewModel
{
    public class ItemViewModel
    {
        public List<Item>? Items { get; set; }
        public SelectList? Categories { get; set; }
        public SelectList? Years { get; set; }
        public string? ItemCategory { get; set; }
        public string? ItemYear { get; set; }
        public string? ItemSearch { get; set; }
    }
}
