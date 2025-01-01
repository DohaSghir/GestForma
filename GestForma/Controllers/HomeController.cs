using System.Diagnostics;
using GestForma.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestForma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult Instructors()
        {
            return View();
        }
        public IActionResult Testimonial()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        // Admin
        public IActionResult AdminDashboard()
        {
            /*if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account");  // Rediriger vers la page de connexion si l'utilisateur n'est pas un admin
            }
            */
            ViewData["Layout"] = "_LayoutAdmin";
            return View();
        }

        //Formateur
        public IActionResult FormateurDashboard()
        {
            /*if (!User.IsInRole("Formateur"))
            {
                return RedirectToAction("Login", "Account");  // Rediriger vers la page de connexion si l'utilisateur n'est pas un formateur
            }*/

            ViewData["Layout"] = "_LayoutFormateur";
            return View();
        }
    }
}
