using Books.Models;
using Books.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class OfferController : Controller
    {
        public readonly DbContext _context;
        private readonly IBookRepository _bookRepository;

        public OfferController(DbContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList();

            var model = new OfferViewModel
            {
                Books = books,
           
                SearchQuery = ""
                
            };

            return View(model);
           
        }
        public IActionResult Index(OfferViewModel _model)
        {
           

            var model = new OfferViewModel
            {
                Books = _model.Books,
                SearchQuery = _model.SearchQuery,
              
                
                
            };

            return View(model);

        }

        public IActionResult Search(string search)
        {
            var books = _context.Books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || 
            b.Author.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var model = new OfferViewModel
            {
                SearchQuery = search,
                Books = books
            };

            return RedirectToAction("Index", model);
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
    }
}
