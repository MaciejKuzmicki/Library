using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class BookLoan
    {
        [Key]
        public int BookLoanId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
