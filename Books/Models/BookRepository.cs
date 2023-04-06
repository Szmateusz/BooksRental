using Books.Services;
using Google;

namespace Books.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly DbContext _context;

        public BookRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }
        public IEnumerable<Book> GetAllAvaibleBooks()
        {
            return _context.Books.Where(b=>b.AvailableCopies>0).ToList();
        }

        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
    }
}
