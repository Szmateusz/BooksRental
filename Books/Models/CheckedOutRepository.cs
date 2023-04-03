using Google;

namespace Books.Models
{
    public class CheckedOutRepository : ICheckedOutRepository
    {
        private readonly DbContext _context;

        public CheckedOutRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<CheckedOut> GetAllCheckedOutBooks()
        {
            return _context.CheckedOuts.ToList();
        }

        public CheckedOut GetCheckedOutById(int id)
        {
            return _context.CheckedOuts.FirstOrDefault(c => c.Id == id);
        }

        public void AddCheckedOut(CheckedOut checkedOut)
        {
            _context.CheckedOuts.Add(checkedOut);
            _context.SaveChanges();
        }

        public void UpdateCheckedOut(CheckedOut checkedOut)
        {
            _context.CheckedOuts.Update(checkedOut);
            _context.SaveChanges();
        }

        public void DeleteCheckedOut(int id)
        {
            var checkedOut = GetCheckedOutById(id);
            _context.CheckedOuts.Remove(checkedOut);
            _context.SaveChanges();
        }
    }
}
