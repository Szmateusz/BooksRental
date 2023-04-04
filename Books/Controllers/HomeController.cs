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

            var model = new HomeIndexViewModel
            {
                LatestBooks = latestBooks,
                RecommendedBooks = recommendedBooks
            };

            return View(model);
        }

    }
}

