using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore; 

using MVC.Models; 
using MVC.Data; 

namespace MVC.Controllers
{
    public class GrafiekController:Controller
    {
        ProductDbContext _context; 
        public GrafiekController(ProductDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            var partialOrders = _context.PartialOrder.GroupBy(g => g.OrderId).Select(s => s.First()).OrderByDescending(o => o.Date); 

            ViewBag.Data = partialOrders.ToArray(); 
            
            var product_ordered = _context.Products.OrderByDescending(p => p.Sold).ToList();
            List<Product> best_sold = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                best_sold.Add(product_ordered[i]);
            }

            return View(best_sold); 
        }

        
    }
}