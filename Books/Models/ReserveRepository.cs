using Books.Services;
using Google;

namespace Books.Models
{
    public class ReserveRepository : IReserveRepository
    {
        private readonly DbContext _context;

        public ReserveRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reserve> GetAllReserveBooks()
        {
            return _context.Reserves.ToList();
        }
        public IEnumerable<Reserve> GetAllUserReserveBooks(string id)
        {
            return _context.Reserves.Where(u=>u.UserId.Equals(id)).ToList();
        }

        public Reserve GetReserveById(int id)
        {
            return _context.Reserves.FirstOrDefault(c => c.Id == id);
        }

        public Reserve GetReserveByBookId(int id)
        {
            return _context.Reserves.FirstOrDefault(x => x.BookId == id);
        }
         

        public void AddReserve(Reserve reserve)
        {
            _context.Reserves.Add(reserve);
            _context.SaveChanges();
        }

        public void UpdateReserve(Reserve reserve)
        {
            _context.Reserves.Update(reserve);
            _context.SaveChanges();
        }

        public void DeleteReserve(int id)
        {
            var reserve = GetReserveById(id);
            _context.Reserves.Remove(reserve);
            _context.SaveChanges();
        }
    }
}
