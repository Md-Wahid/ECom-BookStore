namespace WebBack.Models
{
    public class BookPicture
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PictureUri { get; set; }
        public string PictureName { get; set; }
        public string BookId { get; set; }
        public Book Book { get; set; }
    }
}