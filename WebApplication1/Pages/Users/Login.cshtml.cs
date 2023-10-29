using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authentication;
using WebApplication1.Model;

namespace WebApplication1.Pages.Users
{
    [RequireNoAuth]
    public class LoginModel : PageModel
    {
        private readonly DatabaseContext _context;

        public LoginModel(DatabaseContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string errorMessage { get; set; } = "";

        public async Task OnPostAsync()
        {
            User = await _context.Users.ToListAsync();
            if (Email == null || Password == null) return;
            foreach (var user in User)
            {
                if(Email.Equals(user.Email))
				{
                    var passwordHasher = new PasswordHasher<IdentityUser>();
					var result = passwordHasher.VerifyHashedPassword(new IdentityUser(), user.Password,  Password);
                    if(result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        HttpContext.Session.SetInt32("id",user.UserId );
                        HttpContext.Session.SetString("firstname", user.FirstName);
						HttpContext.Session.SetString("lastname", user.LastName);
						HttpContext.Session.SetString("email", user.Email);
						HttpContext.Session.SetString("role", user.Role);
                        HttpContext.Session.SetString("phonenumber", user.PhoneNumber);

                        Response.Redirect("/");
					}
				}
            }
        }
    }
}
