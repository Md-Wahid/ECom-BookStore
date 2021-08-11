using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebBack.Models;

namespace WebBack.ViewModels
{
    public class BookViewModel
    {
        // [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }

        [Display(Name = "Book Author")]
        public int BookAuthorId { get; set; }

        [Display(Name = "Book Store")]
        public int BookStoreId { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }
        
        [Display(Name = "Category List")]
        public List<Category> Categories { get; set; }
        public List<IFormFile> Pictures { get; set; }
    }
}