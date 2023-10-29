using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class BookCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
