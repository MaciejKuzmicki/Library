using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Model;
using WebApplication1.Authentication;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebApplication1.Pages.Books
{
    [RequireAuth(RequiredRole ="admin")]
    public class AddBookModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public AddBookModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            authors = _context.Authors.ToList();
            categories = _context.BookCategories.ToList();
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        [BindProperty]
        public string errorMessage { get; set; } = "";
        [BindProperty]
        public IList<Author> authors { get; set; }
        [BindProperty]
        public IFormFile formFile { get; set; }
        [BindProperty]
        public string Category1 { get; set; }
        [BindProperty]
        public string Category2 { get; set; }
        [BindProperty]
        public string Category3 { get; set; }
        [BindProperty]
        public List<BookCategory> categories { get; set; }
        [BindProperty]
        public string authorId { get; set; }
        
        

        
        public async Task<IActionResult> OnPostAsync()
        {
            Book.AvailableCopies = 1;
          if (_context.Books == null || Book == null)
            {
                authors = _context.Authors.ToList();
                categories = _context.BookCategories.ToList();
                return Page();
            }
          
            Account account = new Account(); //type your credentials
            Cloudinary cloudinary = new Cloudinary(account);

            if (formFile != null)
            {
                using (var stream = formFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(formFile.FileName, stream),
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);

                    Book.CloudinaryImageId = uploadResult.SecureUri.ToString();
                }
            }

            Book.Categories.Add(_context.BookCategories.FirstOrDefault(c => c.CategoryId == int.Parse(Category1)));
            Book.Categories.Add(_context.BookCategories.FirstOrDefault(c => c.CategoryId == int.Parse(Category2)));
            Book.Categories.Add(_context.BookCategories.FirstOrDefault(c => c.CategoryId == int.Parse(Category3)));
            Book.Author = _context.Authors.FirstOrDefault(c => c.AuthorId == int.Parse(authorId));
            Book.Author.Books.Add(Book);
            _context.Books.Add(Book);
            await Console.Out.WriteLineAsync(authorId);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
