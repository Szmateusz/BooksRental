using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class HomeController : Controller
    {
        public readonly DbContext _context;
        private readonly IBookRepository _bookRepository;

        public HomeController(DbContext context,IBookRepository bookRepository) {
            _context = context;
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var latestBooks = _context.Books.OrderByDescending(b => b.Year).Take(5).ToList();
            var recommendedBooks = _context.Books.Where(b => b.IsRecommended).Take(5).ToList();

            var model = new IndexViewModel
            {
                LatestBooks = latestBooks,
                RecommendedBooks = recommendedBooks
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            // Pobranie wszystkich książek
            var books = _bookRepository.GetAllBooks();

            // Jeśli istnieje szukana fraza, filtruj książki
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b =>
                    b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var model = new IndexViewModel
            {
                SearchedBooks = books.ToList()
            };
            return View(model);
        }

        public IActionResult Offer()
        {
            var books = _context.Books.ToList();

            var model = new OfferViewModel
            {
                Books = books
            };

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        public IActionResult Search(string search)
        {
            var books = _context.Books.Where(b => b.Title.Contains(search) || b.Author.Contains(search)).ToList();

            var model = new SearchViewModel
            {
                SearchQuery = search,
                Books = books
            };

            return View(model);
        }

    }
}

