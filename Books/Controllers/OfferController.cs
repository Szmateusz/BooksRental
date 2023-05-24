
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
        public IActionResult Index(SearchModel? search)
        {

            var books = _bookRepository.GetAllBooks();

            if (!string.IsNullOrEmpty(search.SearchQuery))
            {
                books = books.Where(b => b.Title.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
               b.Author.Contains(search.SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!search.Genres.ToString().Equals("All"))
            {
                books = books.Where(b => b.Genre.Equals(search.Genres)).ToList();
            }

            var model = new OfferViewModel
            {
                Books = books.ToList(),
                SearchQuery = search.SearchQuery ?? ""
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

        [Authorize(AuthenticationSchemes = "Identity.Application")]
       
        public IActionResult Rent(int id)
        {
            

            var userId = _userManager.GetUserId(User);
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.AvailableCopies > 0)
            {
                book.AvailableCopies += -1;

                _bookRepository.UpdateBook(book);

                var rental = new Rental
                {
                    RentalDate = DateTime.Now,
                    BookId = book.Id,
                    UserId = userId,
                    DueDate = DateTime.Now.AddDays(14)
                    
                };

                _rentalRepository.AddRental(rental);
            } else { return NotFound(); }

            return RedirectToAction("Details","Offer", new { id = id });

        }

        [Authorize(AuthenticationSchemes = "Identity.Application")]
        public IActionResult Reserve(int id)
        {
            
            var userId = _userManager.GetUserId(User);

            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if(book == null)
            {
                return NotFound();
            }

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

            var CurrentRentals = _rentalRepository.GetAllUserCurrentRentalBooks(userId).ToList();
            var HistoryRentals = _rentalRepository.GetAllUserRentalBooks(userId).ToList();
            var OverdueRentals = _rentalRepository.GetAllUserOverdueRentalBooks(userId).ToList();

            var model = new UserRentalsViewModel
            {
                CurrentRentals = CurrentRentals,
                RentalsHistory = HistoryRentals,
                RentalsOverdue = OverdueRentals
            };

            return View(model);

        }
        public IActionResult UserReserves()
        {
            var userId = _userManager.GetUserId(User);

            var reserves = _reserveRepository.GetAllUserReserveBooks(userId).ToList();

            return View(reserves);
        }
        public IActionResult DeleteUserReserve(int id)
        {         
            _reserveRepository.DeleteReserve(id);

            return RedirectToAction("UserReserves","Offer");
        }
    }
}
