using System.Collections.Generic;

namespace WebBack.Models
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}