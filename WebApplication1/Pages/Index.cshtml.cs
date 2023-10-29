using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly WebApplication1.Model.DatabaseContext _context;


        public IndexModel(ILogger<IndexModel> logger, WebApplication1.Model.DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public IList<Author> authors { get; set; }
        [BindProperty]
        public IList<Book> books { get; set; }

        public void OnGet()
        {
            authors = _context.Authors.ToList().Take(3).ToList();
            books = _context.Books.ToList();
        }
    }
}