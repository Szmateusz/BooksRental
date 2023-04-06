using Books.Services;
using Google;

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
            return _context.Rentals.ToList();
        }
        public IEnumerable<Rental> GetAllUserRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).ToList();
        }
        public IEnumerable<Rental> GetAllUserCurrentRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).Where(x=>x.ReturnDate>DateTime.Now).ToList();

        }
        public IEnumerable<Rental> GetAllUserOverdueRentalBooks(string id)
        {
            return _context.Rentals.Where(u => u.UserId.Equals(id)).Where(x=>x.DueDate<DateTime.Now).ToList();

        }

        public Rental GetRentalById(int id)
        {
            return _context.Rentals.FirstOrDefault(c => c.Id == id);
        }

        public void AddRental(Rental rental)
        {
            _context.Rentals.Add(rental);
            _context.SaveChanges();
        }

        public void UpdateRental(Rental rental)
        {
            _context.Rentals.Update(rental);
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
