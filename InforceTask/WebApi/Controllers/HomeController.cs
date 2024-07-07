using System.Diagnostics;
using InforceTask.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTask.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var aboutContent = _context.AboutContents.FirstOrDefault();
            return View(aboutContent);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        
        public IActionResult EditAbout(string description)
        {
            var aboutContent = _context.AboutContents.FirstOrDefault();
            if (aboutContent == null)
            {
                aboutContent = new AboutContent { Description = description };
                _context.AboutContents.Add(aboutContent);
            }
            else
            {
                aboutContent.Description = description;
                _context.AboutContents.Update(aboutContent);
            }

            _context.SaveChanges();
            return RedirectToAction("About");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}