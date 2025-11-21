// Controllers/HomeController.cs
using Microsoft.EntityFrameworkCore;
using ContractClaims.Data;
using ContractClaims.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ContractClaims.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
