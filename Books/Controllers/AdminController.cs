using Books.Models;
using Books.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Books.Controllers
{
    [Authorize(Roles = "admin", AuthenticationSchemes = "Identity.Application")]
    public class AdminController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IReserveRepository _reserveRepository;
        public readonly DbContext _context;

        public AdminController(IBookRepository bookRepository, IRentalRepository rentalRepository, DbContext context, IReserveRepository reserveRepository)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
            _context = context;
            _reserveRepository = reserveRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAllBooks().ToList();
            var rentals = _rentalRepository.GetAllRentalBooks().ToList();

            var model = new AdminIndexViewModel
            {
                Books = books,
                Rentals = rentals

            };

            return View(model);
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
        public IActionResult OverdueRentals()
        {
            var rentals = _rentalRepository.GetAllOverdueRentalBooks().ToList();

            return View(rentals);
        }
        public IActionResult ViewUsers()
        {
            var users = _context.Users.ToList();

            return View(users);
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

            return RedirectToAction("Index","Admin");
        }

        public IActionResult ReturnBook(string userId)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(userId));

            if (user == null)
            {
                return NotFound();
            }

            user.Rentals = _rentalRepository.GetAllUserRentalBooks(userId).Where(x=>x.ReturnDate==null).ToList();
            
            return View(user);
        }

        public IActionResult ReturnBook(int rentalId)
        {
            var rental = _rentalRepository.GetRentalById(rentalId);

            string userId=rental.UserId;

            if (rental == null)
            {
                return NotFound();
            }
            
             rental.ReturnDate = DateTime.Now;
            
            _rentalRepository.UpdateRental(rental);

            return RedirectToAction("ReturnBook", "Admin", userId);


        }
        public IActionResult Remind(int rentalId)
        {
            var rental = _rentalRepository.GetRentalById(rentalId);

            if (rental.DueDate < DateTime.Today)
            {
                var user = rental.User;
                var book = rental.Book;
                
                
                var emailSubject = "Przypomnienie o zwrocie książki";
                var emailMessage = $"Wypożyczenie książki \"{book.Title}\" przez klienta {user.FirstName} {user.LastName} minęło {Math.Round((DateTime.Now - rental.DueDate).TotalDays)} dni temu termin zwrotu był na {rental.DueDate.ToString("dd-MM-yyyy")} . Prosimy o jak najszybszy zwrot książki.";

                if(EmailSender.SendEmail(user.Email, emailSubject, emailMessage))
                {
                    var result = $"Email do {user.FirstName} {user.LastName} został wysłany";

                    return Json(new { success = true, result });
                }
                else
                {
                    var result = $"Email nie został wysłany!";
                    return Json(new { success = false, result });
                }
          
            }

            return NotFound();
        }

        public IActionResult ViewBorrowedBooks()
        {
            return View();
        }

        public IActionResult ViewReservedBooks()
        {
           var model = _reserveRepository.GetAllReserveBooks().ToList();
            return View(model);
        }
        public IActionResult DeleteReserv(int id)
        {

            _reserveRepository.DeleteReserve(id);

            return RedirectToAction("ViewReservedBooks","Admin");
        }

        public IActionResult ViewOverdueBooks()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(string userId)
        {
            var model = _context.Users.SingleOrDefault(x => x.Id.Equals(userId));
            return View(model);

        }
        [HttpPost]
        public IActionResult EditUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(x => x.Id.Equals(model.Id));

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.DateOfBirth = model.DateOfBirth;

                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction("ViewUsers", "Admin");
                }
                

            }
            return View(model);

        }
    }
}

