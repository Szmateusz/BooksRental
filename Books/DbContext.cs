
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
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Reserve> Reserves { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().Property(u => u.FirstName).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.LastName).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Email).IsRequired();

            modelBuilder.Entity<Rental>()
            .HasKey(u => u.Id);

            modelBuilder.Entity<Rental>()
            .HasOne(r => r.User)
            .WithMany(u => u.Rentals)
            .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rental>()
            .HasKey(c => new { c.BookId, c.UserId });

            modelBuilder.Entity<Rental>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId);

           
        }
    }
}
