using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.BookReviews
{
    [RequireAuth(RequiredRole = "client")]
    public class EditBookReviewModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public EditBookReviewModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookReview BookReview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.BookReviews == null)
            {
                return NotFound();
            }

            var bookreview =  await _context.BookReviews.FirstOrDefaultAsync(m => m.ReviewId == id);
            if (bookreview == null)
            {
                return NotFound();
            }
            BookReview = bookreview;
           ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId");
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync()
        {

            BookReview.ReviewDate = DateTime.UtcNow;
            _context.Attach(BookReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookReviewExists(BookReview.ReviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }

        private bool BookReviewExists(int id)
        {
          return (_context.BookReviews?.Any(e => e.ReviewId == id)).GetValueOrDefault();
        }
    }
}
