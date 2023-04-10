using Books.Services;
using Google;
using Microsoft.EntityFrameworkCore;

namespace Books.Models
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DbContext _context;

        public RentalRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<Rental> GetAllRentalBooks()
        {
            return _context.Rentals.Include(u => u.User).Include(b=>b.Book).ToList();
        }
        public IEnumerable<Rental> GetAllCurrentRentalBooks()
        {
            return _context.Rentals.Where(x=>x.ReturnDate==null).Include(u => u.User).Include(b => b.Book).ToList();

        }

        public IEnumerable<Rental> GetAllOverdueRentalBooks()
        {

            return _context.Rentals.Where(x => x.DueDate < DateTime.Now && x.ReturnDate == null).Include(b => b.Book).Include(u=>u.User).ToList();

        }

        public IEnumerable<Rental> GetAllUserRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).Include(b=>b.Book).ToList();
        }
        public IEnumerable<Rental> GetAllUserCurrentRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).Where(x=>x.DueDate > DateTime.Now).Include(b=>b.Book).ToList();

        }
        public IEnumerable<Rental> GetAllUserOverdueRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).Where(x=>x.DueDate < DateTime.Now).Include(b => b.Book).ToList();

        }

        public Rental GetRentalById(int id)
        {
            return _context.Rentals.Include(u => u.User).Include(b=>b.Book).FirstOrDefault(c => c.Id == id);
        }

        public void AddRental(Rental rental)
        {
    
            _context.Rentals.Add(rental).Property(x => x.Id).IsModified = false;
            _context.SaveChanges();
        }

        public void UpdateRental(Rental rental)
        {

            _context.Update(rental).Property(x => x.Id).IsModified = false;
            _context.SaveChanges();
        }

        public void DeleteRental(int id)
        {
            var rental = GetRentalById(id);
            _context.Rentals.Remove(rental);
            _context.SaveChanges();
        }
    }
}
