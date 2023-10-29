using System.ComponentModel.DataAnnotations;
using System;

namespace WebApplication1.Model
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public int PublicationYear { get; set; }
        public string ISBN { get; set; }
        public int AvailableCopies { get; set; }
        public string CloudinaryImageId { get; set; } 
        public string BookDescription { get; set; }

        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
        public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
        public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
    }
}
