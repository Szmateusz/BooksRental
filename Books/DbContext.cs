
using Books.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Books
{
    public class DbContext : IdentityDbContext<UserModel>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CheckedOut> CheckedOuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().Property(u => u.FirstName).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.LastName).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Email).IsRequired();

            modelBuilder.Entity<CheckedOut>()
            .HasKey(c => new { c.BookId, c.UserId });

            modelBuilder.Entity<CheckedOut>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId);

           
        }
    }
}
