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
    public class GrafiekController : Controller
    {
        ProductDbContext _context;
        public GrafiekController(ProductDbContext context){
            _context = context;
        }

        public IActionResult Index()
        {
            var partialOrders = _context.PartialOrder.GroupBy(g => g.OrderId).Select(s => s.First());

            ViewBag.Data = partialOrders.ToArray();

            return View();
        }
    }
}