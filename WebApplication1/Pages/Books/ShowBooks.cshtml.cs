using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WebApplication1.Model;

namespace WebApplication1.Pages.Books
{
    [BindProperties(SupportsGet = true)]
    public class ShowBooksModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public ShowBooksModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        public String Title { get; set; } 
        public String Category { get; set; } 

        public IList<Book> Book { get;set; } = default!;
        public IList<Book> Books { get; set; } = default!;
        public IList<Book> Books2 { get; set; } = default!;
        public IList<BookCategory> BookCategories { get; set; } = default!;


        public async Task OnGetAsync()
        {
            if (_context.Books != null)
            {
                Books = await _context.Books.Include(b=>b.Categories).Include(b=>b.Author).ToListAsync();
                BookCategories = await _context.BookCategories.ToListAsync();

            }
            if (!string.IsNullOrEmpty(Category))
            {
                // Filter books by category
                Books2 = Books.Where(book => book.Categories.Any(c => c.CategoryName == Category)).ToList();
                await Console.Out.WriteLineAsync("HERE1");
            }
            else
            {
                Books2 = Books;
                await Console.Out.WriteLineAsync("HERE2");

            }

            if (!string.IsNullOrEmpty(Title))
            {
                // Filter books by title
                Book.AddRange(Books2.Where(book => book.Title.Contains(Title)));
                await Console.Out.WriteLineAsync("HERE3");

            }
            else
            {
                Book = Books2;
                await Console.Out.WriteLineAsync("HERE4");

            }
            await Console.Out.WriteLineAsync(Title);
            await Console.Out.WriteLineAsync(Category);



        }
    }
}
