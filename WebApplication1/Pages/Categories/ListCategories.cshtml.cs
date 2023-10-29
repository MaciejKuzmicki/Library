using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.Categories
{
    [RequireAuth(RequiredRole ="admin")]
    public class ListCategoriesModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public ListCategoriesModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        public IList<BookCategory> BookCategory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BookCategories != null)
            {
                BookCategory = await _context.BookCategories.ToListAsync();
            }
        }
    }
}
