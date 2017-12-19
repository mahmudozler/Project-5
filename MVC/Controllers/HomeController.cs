using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using MVC.Data;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDbContext _context;

        public HomeController(ProductDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            updateSold();

            var product_ordered = _context.Products.OrderByDescending(p => p.Sold).ToList();
            List<Product> best_sold = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                best_sold.Add(product_ordered[i]);
            }

            return View(best_sold);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your Application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void updateSold()
        {
            DateTime today = DateTime.Now.Date;

            foreach (var order in _context.PartialOrder.GroupBy(p => p.OrderId)
                                                       .Select(g => g.First())
                                                       .Where(p => (today - p.Date).TotalDays <= 7))
            {
                var temp_product = _context.Products.Where(p => p.Id == order.ProductId).FirstOrDefault();

                temp_product.Sold = order.Amount;
                _context.SaveChanges();
            }
        }
    }
}
