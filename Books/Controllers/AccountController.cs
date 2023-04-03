using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
