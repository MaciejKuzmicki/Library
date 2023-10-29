using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.Categories
{
    [RequireAuth(RequiredRole ="admin")]
    public class AddCategoryModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public AddCategoryModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BookCategory BookCategory { get; set; } = default!;
        [BindProperty]
        public string errorMessage { get; set; } = "";
        

        
        public async Task<IActionResult> OnPostAsync()
        {
          if (_context.BookCategories == null || BookCategory == null)
            {
                return Page();
            }
          foreach(var bookCategory in _context.BookCategories)
            {
                if (bookCategory.CategoryName == BookCategory.CategoryName) {
                    errorMessage = "There exists this category";
                    return Page();
                }
            }
          
            _context.BookCategories.Add(BookCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
