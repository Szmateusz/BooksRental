using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Books.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICheckedOutRepository _checkOutRepository;
        public readonly DbContext _context;

        public AdminController(IBookRepository bookRepository, ICheckedOutRepository checkOutRepository, DbContext context)
        {
            _bookRepository = bookRepository;
            _checkOutRepository = checkOutRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
       


        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _bookRepository.AddBook(book);
            return RedirectToAction("Index");
        }

        public IActionResult EditBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult EditBook(Book book)
        {
            _bookRepository.UpdateBook(book);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
            return RedirectToAction("Index");
        }

        public IActionResult CheckOuts()
        {
            var checkOuts = _checkOutRepository.GetAllCheckedOutBooks();
            return View(checkOuts);
        }

        public IActionResult BorrowBook()
        {
            return View();
        }

        public IActionResult ReturnBook()
        {
            return View();
        }
        public IActionResult ReturnBook(int id)
        {
            var rental = _context.CheckedOuts.FirstOrDefault(r => r.BookId == id && !r.DateReturned.HasValue);

            if (rental == null)
            {
                return NotFound();
            }

            rental.DateReturned = DateTime.Now;
            _context.SaveChanges();

            if (rental.DueDate < DateTime.Today)
            {
                var customer = rental.User;
                var book = rental.Book;

                var emailSubject = "Przypomnienie o zwrocie książki";
                var emailMessage = $"Wypożyczenie książki \"{book.Title}\" przez klienta {customer.FirstName} {customer.LastName} minęło termin zwrotu. Prosimy o jak najszybszy zwrot książki.";

                EmailSender.SendEmail(customer.Email, emailSubject, emailMessage);
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult ViewBorrowedBooks()
        {
            return View();
        }

        public IActionResult ViewReservedBooks()
        {
            return View();
        }

        public IActionResult ViewOverdueBooks()
        {
            return View();
        }
        public IActionResult ViewUsers()
        {
            return View();
        }
        public IActionResult ViewEditUser()
        {
            return View();

        }
    }
}

