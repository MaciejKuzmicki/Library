using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Pages.Books
{
    public class ShowBookModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public ShowBookModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Book Book { get; set; } = default!;
        [BindProperty]
        public User User { get; set; } = default!;
        public string role { get; set; }
        [BindProperty]
        public string errorMessage { get; set; } = "";
        [BindProperty]
        public int SomeValue { get; set; }
        [BindProperty]
        public bool wasLoaned { get; set; } = false;
        [BindProperty]
        public string opinion { get; set; }
        [BindProperty]
        public bool wasReviewed { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            role = HttpContext.Session.GetString("role");
            SomeValue = (int)id;
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .Include(b=>b.Reviews)
                .ThenInclude(b=>b.User)
                .SingleOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                Book = book;
                await Console.Out.WriteLineAsync("Count: " + Book.Reviews.Count() + " Number");
            }

            
            if (HttpContext.Session.TryGetValue("role", out _))
            {
                int userId = (int)HttpContext.Session.GetInt32("id");
                User = await _context.Users
                    .Include(u => u.BookLoans)
                    .FirstOrDefaultAsync(m => m.UserId == userId);

                foreach (var bookloan in User.BookLoans)
                {
                    if (bookloan.Book != null) // Check if bookloan.Book is not null
                    {
                        if (bookloan.Book.BookId == id)
                        {
                            wasLoaned = true;
                        }

                        // Load Reviews for the specific bookloan explicitly
                        _context.Entry(bookloan.Book).Collection(b => b.Reviews).Load();

                        if (bookloan.Book.Reviews != null && bookloan.Book.Reviews.Count() == 1)
                        {
                            wasReviewed = true;
                        }
                    }
                }

            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string handler)
        {
            int someValue = int.Parse(Request.Form["someValue"]);

            if (handler == "handler1")
            {
                BookLoan bookLoan = new BookLoan();
                bookLoan.Book = _context.Books.Include(b => b.Author).SingleOrDefault(b => b.BookId == someValue);
                bookLoan.LoanDate = DateTime.UtcNow;
                bookLoan.ReturnDate = bookLoan.LoanDate;
                bookLoan.User = await _context.Users.FirstOrDefaultAsync(m=> m.UserId == (int)HttpContext.Session.GetInt32("id"));
                bookLoan.Book.AvailableCopies = 0;
                bookLoan.Book.BookLoans.Add(bookLoan);

                bookLoan.User.BookLoans.Add(bookLoan);

                _context.BookLoans.Add(bookLoan);
                Book = _context.Books.Include(b => b.Author).SingleOrDefault(b => b.BookId == someValue);
                User = bookLoan.User;
                await _context.SaveChangesAsync();
                errorMessage = "Succesfully loaned the book!";
            }
            
            var book = _context.Books.Include(b => b.Author).Include(b => b.Categories).SingleOrDefault(b => b.BookId == someValue);
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                Book = book;
            }

            return Page();

        }



    }



}
