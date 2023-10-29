using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookCategory> BookCategories {  get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<Book> Books { get; set; }
    }


}
