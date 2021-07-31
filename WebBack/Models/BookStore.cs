using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBack.Models
{
    public class BookStore
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}