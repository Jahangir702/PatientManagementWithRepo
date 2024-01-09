using MasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MasterDetails.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
