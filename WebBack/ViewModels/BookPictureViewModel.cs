using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebBack.Models;

namespace WebBack.ViewModels
{
    public class BookPictureViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile Picture { get; set; }
        public string PictureUri { get; set; }
        public string PictureName { get; set; }
        public string BookId { get; set; }
        public Book Book { get; set; }
        // public List<BookCategory> BookCategories { get; set; }
        public List<Category> Categories { get; set; }
    }
}