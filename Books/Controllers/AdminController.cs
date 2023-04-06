using Books.Models;
using Books.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Books.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        public readonly DbContext _context;

        public AdminController(IBookRepository bookRepository, IRentalRepository rentalRepository, DbContext context)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
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

        public IActionResult Rentals()
        {
            var rentals = _rentalRepository.GetAllRentalBooks().ToList();

            return View(rentals);
        }

        [HttpGet]
        public IActionResult BorrowBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);

            var users = _context.Users.ToList();

            var model = new BorrowBookModel
            {
                Users = users,

                Rental = new Rental
                {
                    BookId = bookId,
                    Book = book


                }
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult BorrowBook(BorrowBookModel model,DateTime date)
        {
            var rental = new Rental
            {
                RentalDate = DateTime.Now,
                BookId = model.Rental.BookId,
                UserId = model.Users.First().Id,
                DueDate = date
                
            };

            var book = model.Rental.Book;
            book.AvailableCopies += -1;

            _bookRepository.UpdateBook(book);

            _rentalRepository.AddRental(rental);

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

