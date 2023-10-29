using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Authentication;
using WebApplication1.Enums;
using WebApplication1.Model;

namespace WebApplication1.Pages.Authors
{
    [RequireAuth(RequiredRole = "admin")]
    public class AddAuthorModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public AddAuthorModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<Country> countryList {  get; set; } 

        public IActionResult OnGet()
        {
            countryList = Enum.GetValues(typeof(Country)).Cast<Country>().ToList();
            return Page();
        }

        [BindProperty]
        public Author Author { get; set; } = default!;
        [BindProperty]
        public string errorMessage { get; set; } = "";
        [BindProperty]
        public IFormFile Photo { get; set; }

        
        
        public async Task<IActionResult> OnPostAsync()
        {
            Account account = new Account(); //type the credentials
            Cloudinary cloudinary = new Cloudinary(account);
            if (Photo != null)
            {
                using (var stream = Photo.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(Photo.FileName, stream),
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);

                    Author.CloudinaryId = uploadResult.SecureUri.ToString();
                }
            }
            else
            {
                countryList = Enum.GetValues(typeof(Country)).Cast<Country>().ToList();
                return Page();
            }

            if (_context.Authors == null || Author == null)
            {
                return Page();
            }
            Author.Books = new List<Book>();
            _context.Authors.Add(Author);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
