using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        
		private readonly ProductContext _context;

		public HomeController(ProductContext context)
		{
			_context = context;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

		public async Task<IActionResult> Producten()
		{
			//ViewData["Message"] = "Your contact page.";

            return View(await _context.products.ToListAsync());
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
