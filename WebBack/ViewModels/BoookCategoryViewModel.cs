using System.Collections.Generic;
using WebBack.Models;

namespace WebBack.ViewModels
{
    public class BoookCategoryViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public Book Book { get; set; }
        // public int CategoryId { get; set; }
        // public Category Category { get; set; }
        public List<Category> Categories { get; set; }
    }
}