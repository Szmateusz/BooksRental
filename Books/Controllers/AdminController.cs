using Books.Models;
using Books.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Books.Controllers
{
    [Authorize(Roles = "admin", AuthenticationSchemes = "Identity.Application")]
    public class AdminController : Controller
    {
        public readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<UserModel> _usermanager;
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IReserveRepository _reserveRepository;
        public readonly DbContext _context;

        public AdminController(IBookRepository bookRepository, IRentalRepository rentalRepository, DbContext context, IReserveRepository reserveRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,UserManager<UserModel> usermanager)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
            _context = context;
            _reserveRepository = reserveRepository;
            _hostingEnvironment = hostingEnvironment;   
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewCurrentRentals()
        {
           
            var rentals = _rentalRepository.GetAllCurrentRentalBooks().ToList();

            var model = new AdminViewCurrentRentals
            {
                Rentals = rentals,
                SearchQuery = ""
            };
           
            return View(model);
        }
        [HttpPost]
        public IActionResult ViewCurrentRentals(AdminViewCurrentRentals search)
        {

            var rentals = _rentalRepository.GetAllCurrentRentalBooks().ToList();

            if (!string.IsNullOrEmpty(search.SearchQuery))
            {
                rentals = rentals.Where(b => b.User.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.Book.Title.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.Email.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = new AdminViewCurrentRentals
            {
                Rentals = rentals,
                SearchQuery = search.SearchQuery
            };

            return View(model);
        }



        [HttpGet]
        public IActionResult ViewBooks()
        {
            var books = _bookRepository.GetAllBooks().ToList();

            var model = new AdminViewBooksModel
            {

                Books = books,
                SearchQuery = ""

            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ViewBooks(AdminViewBooksModel search)
        {

            var books = _bookRepository.GetAllBooks().ToList();

            if(!string.IsNullOrEmpty(search.SearchQuery))
            {
                books = books.Where(b => b.Title.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
               b.Author.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }
           
            if (!search.Genres.ToString().Equals("All"))
            {
                books = books.Where(b => b.Genre.Equals(search.Genres.ToString())).ToList();
            }

            var model = new AdminViewBooksModel
            {

                Books = books,
                SearchQuery = search.SearchQuery

            };

            return View(model);
        }



        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book, IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                var fileName = Path.GetFileName(book.Title + ".jpg");
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                // Zapisz nazwę pliku w modelu
                book.Image = fileName;
            }
            else { book.Image = "default.png"; }


            _bookRepository.AddBook(book);
            return RedirectToAction("Index");
        }

        public IActionResult EditBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(Book book,IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                var fileName = Path.GetFileName(book.Title + ".jpg");
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                // Zapisz nazwę pliku w modelu
                book.Image = fileName;
            }
            else { book.Image = "default.png"; }



            _bookRepository.UpdateBook(book);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
            return RedirectToAction("Index");
        }

        
        public IActionResult OverdueRentals()
        {
            var rentals = _rentalRepository.GetAllOverdueRentalBooks().ToList();

            return View(rentals);
        }
        [HttpGet]
        public IActionResult ViewUsers()
        {
            var users = _context.Users.ToList();

            var model = new AdminViewUsers
            {
                Users = users,
                SearchQuery = ""
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ViewUsers(AdminViewUsers search)
        {
            var users = _context.Users.ToList();

            if (!string.IsNullOrEmpty(search.SearchQuery))
            {
               users = users.Where(b => b.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                b.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
               b.Email.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

         

            var model = new AdminViewUsers
            {

                Users = users,
                SearchQuery = search.SearchQuery

            };

            return View(model);

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

            var book = _bookRepository.GetBookById(model.Rental.BookId);

            var rental = new Rental
            {
                RentalDate = DateTime.Now,
                BookId = book.Id,
                UserId = model.UserId,
                DueDate = date
                
            };

            
            book.AvailableCopies += -1;
            
            _bookRepository.UpdateBook(book);

            _rentalRepository.AddRental(rental);

            return RedirectToAction("Index","Admin");
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult ReturnBook(int rentalId)
        {
            var rental = _rentalRepository.GetRentalById(rentalId);

            if (rental == null)
            {
                return NotFound();
            }

                
             rental.ReturnDate = DateTime.Now;
            
            _rentalRepository.UpdateRental(rental);

            //Add avaible copie

            var book = rental.Book;
            book.AvailableCopies += +1;

            _bookRepository.UpdateBook(book);


            var result = $"Książka została zwrócona";

            return Json(new { success = true, result });


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
            else
            {
                var result = $"Termin zwrotu nie upłynął!";
                return Json(new { success = false, result });
            }

            return NotFound();
           
        }

        [HttpGet]
        public IActionResult ViewRentalBooks()
        {
            var rentals = _rentalRepository.GetAllRentalBooks().Where(x=>x.ReturnDate != null).ToList();

            var model = new AdminViewRentalBooks
            {
                Rentals = rentals,
                SearchQuery = ""
            };

            return View(model);

        }
        [HttpPost]
        public IActionResult ViewRentalBooks(AdminViewRentalBooks search)
        {
            var rentals = _rentalRepository.GetAllRentalBooks().Where(x => x.ReturnDate != null).ToList();


            if (!string.IsNullOrEmpty(search.SearchQuery))
            {
                rentals = rentals.Where(b => b.Book.Title.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.LastName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                b.User.Email.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = new AdminViewRentalBooks
            {
                Rentals = rentals,
                SearchQuery = search.SearchQuery
            };

            return View(model);

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

        [HttpGet]
        public IActionResult ViewOverdueBooks()
        {
            var rentals = _rentalRepository.GetAllOverdueRentalBooks().ToList();

            var model = new AdminViewOverdueBooks
            {
                Rentals = rentals,
                SearchQuery = ""
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult ViewOverdueBooks(AdminViewOverdueBooks search)
        {
            var rentals = _rentalRepository.GetAllOverdueRentalBooks().ToList();


            if (!string.IsNullOrEmpty(search.SearchQuery))
            {
                rentals = rentals.Where(b => b.Book.Title.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.FirstName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                 b.User.LastName.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                b.User.Email.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = new AdminViewOverdueBooks
            {
                Rentals = rentals,
                SearchQuery = search.SearchQuery
            };

            return View(model);
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

        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewData["isRegistered"] = false;
            ViewData["isPasswordLenght"] = true;

            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateUser(RegisterModel userRegisterData)
        {
            ViewData["isPasswordLenght"] = true;
            ViewData["isRegistered"] = false;


            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }
            if (_context.Users.Any(x => x.UserName.Equals(userRegisterData.UserName)))
            {
                ViewData["isRegistered"] = true;

                return View(userRegisterData);
            }
            if (userRegisterData.Password.Length < 5)
            {
                ViewData["isPasswordLenght"] = false;
                return View(userRegisterData);

            }

            var newUser = new UserModel
            {
                UserName = userRegisterData.UserName,
                FirstName = userRegisterData.FirstName,
                LastName = userRegisterData.LastName,
                Email = userRegisterData.Email,

                DateOfBirth = userRegisterData.DateOfBirth
            };

            await _usermanager.CreateAsync(newUser, userRegisterData.Password);

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeleteUser(string id)
        {
            var user =  _context.Users.SingleOrDefault(u=>u.Id.Equals(id));
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }  
            return RedirectToAction("ViewUsers");
        }
    }
}

