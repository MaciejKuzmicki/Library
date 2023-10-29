using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.Users
{
    [RequireNoAuth]
    public class RegisterModel : PageModel
    {
        private readonly WebApplication1.Model.DatabaseContext _context;

        public RegisterModel(WebApplication1.Model.DatabaseContext context)
        {
            _context = context;
            
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;
        [BindProperty]
        public string errorMessage { get; set; } = "";

       
        public async Task<IActionResult> OnPostAsync()
        {
            
          if (User == null)
          { 
            return Page();
          }
          if(!User.Email.Contains("@"))
            {
                errorMessage = "E-mail is not correct";
                return Page();
            }
            User.Role = "client";
            User.RegistrationDate = DateTime.UtcNow;
            User.BookLoans = null; 
            
            foreach(var user in _context.Users)
            {
                if(user.Email == User.Email)
                {
                    errorMessage = "There is an user with the same e-mail address";
                    return Page();
                }
                else if(user.PhoneNumber == User.PhoneNumber)
                {
                    errorMessage = "There is an user with the same phone number";
                    return Page();
                }
            }
            var passwordHasher = new PasswordHasher<IdentityUser>();
            User.Password = passwordHasher.HashPassword(new IdentityUser(), User.Password);
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
