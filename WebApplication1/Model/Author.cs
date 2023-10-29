using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Biography { get; set; }
        public string CloudinaryId { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
