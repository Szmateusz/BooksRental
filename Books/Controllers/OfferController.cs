
using Books.Models;
using Books.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class OfferController : Controller
    {
        public readonly DbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IReserveRepository _reserveRepository;

        public readonly UserManager<UserModel> _userManager;


        public OfferController(DbContext context, IBookRepository bookRepository, IRentalRepository rentalRepository,UserManager<UserModel> userManager, IReserveRepository reserveRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
            _userManager = userManager;
            _reserveRepository = reserveRepository;
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult Search(SearchModel search)
        {
            var books = _context.Books.Where(b => b.Title.Contains(search.SearchQuery) || 
            b.Author.Contains(search.SearchQuery)).ToList();

            if (!search.Genres.ToString().Equals("All"))
            {
                books = books.Where(b => b.Genre.Equals(search.Genres.ToString())).ToList();
            }

            var model = new OfferViewModel
            {
               
                Books = books,
                SearchQuery = search.SearchQuery
              
            };
            return View("Index",model);
            
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

        [Authorize]
        public IActionResult Rent(int id)
        {
            var userId = _userManager.GetUserId(User);
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book.AvailableCopies > 0)
            {
                book.AvailableCopies += -1;

                _bookRepository.UpdateBook(book);

                var rental = new Rental
                {
                    RentalDate = DateTime.Now,
                    BookId = book.Id,
                    UserId = userId,
                    ReturnDate = DateTime.Now.AddDays(14)
                    
                };

                _rentalRepository.AddRental(rental);
            } else { return NotFound(); }

            return RedirectToAction("Details","Offer",id);

        }

        [Authorize]
        public IActionResult Reserve(int id)
        {
            
            var userId = _userManager.GetUserId(User);
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book.AvailableCopies < 1)
            {



                var reserve = new Reserve
                {
                    ReserveDate = DateTime.Now,
                    BookId = book.Id,
                    UserId = userId


                };

                _reserveRepository.AddReserve(reserve);
            }  else { return NotFound(); }


            return RedirectToAction("Details", "Offer", id);       

        }

        public IActionResult UserRentals()
        {
            var userId = _userManager.GetUserId(User);

            var rentals = _rentalRepository.GetAllUserRentalBooks(userId);

            return View(rentals);

        }
        public IActionResult UserReserves()
        {
            var userId = _userManager.GetUserId(User);

            var reserves = _reserveRepository.GetAllUserReserveBooks(userId);

            return View(reserves);
        }
    }
}
