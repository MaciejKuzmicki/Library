using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.BookReviews
{
    [RequireAuth(RequiredRole ="client")]
    public class AddBookReviewModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public AddBookReviewModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
        ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            SomeValue = (int)id;
            return Page();
        }

        [BindProperty]
        public BookReview BookReview { get; set; } = default!;
        [BindProperty]
        public int SomeValue { get; set; }
        [BindProperty]
        public string Opinion {  get; set; }
        [BindProperty]
        public int Rating { get; set; }


        
        public async Task<IActionResult> OnPostAsync()
        {
          if ( _context.BookReviews == null || BookReview == null)
            {
                return Page();
            }
            int someValue = int.Parse(Request.Form["someValue"]);
            User User = await _context.Users.Include(b => b.BookLoans).ThenInclude(b => b.Book).ThenInclude(b => b.Reviews).FirstOrDefaultAsync(m => m.UserId == (int) HttpContext.Session.GetInt32("id"));
            BookLoan bookloan = User.BookLoans.FirstOrDefault(b=>b.BookLoanId == someValue);
            BookReview bookReview = new BookReview();
            bookReview.ReviewText = Opinion;
            bookReview.Book = bookloan.Book;
            bookReview.User = User;
            bookReview.ReviewDate = DateTime.UtcNow;
            bookReview.Rating = Rating;
            bookloan.Book.Reviews.Add(bookReview);


            _context.BookReviews.Add(bookReview);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
