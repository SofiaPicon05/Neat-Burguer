using Microsoft.AspNetCore.Mvc;
using Neat_Burguer.Areas.Admin.Models.ViewModels;
using Neat_Burguer.Models.Entities;
using Neat_Burguer.Repositories;

namespace Neat_Burguer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
