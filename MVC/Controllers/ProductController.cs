using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MVC.Models;
using MVC.Data;
using MVC.Services;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    //[Authorize] [Authorize("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public IActionResult Index(string searchString, int pageIndex = 1)
        {
            ViewData["Message"] = "";

            var res = _context.Products.GetPage<Product>(pageIndex - 1, 24, p => p.Id);

            ViewBag.searchString = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                res = _context.Products.GetPage<Product>(pageIndex - 1, 24, p => p.Id,
                                                                                p => p.Name.ToLower().Contains(searchString.ToLower()) ||
                                                                                p.Type.ToLower().Contains(searchString.ToLower()));
                ViewBag.searchString = "&searchString=" + searchString;

                ViewData["Message"] = "Resultaten voor " + "\"" + searchString + "\"";
            }

            if (res == null) { return NotFound(); }

            ViewBag.pageIndex = pageIndex;
            ViewBag.totalPages = res.totalPages + 1;

            return View(res.items.ToList());
            //return View();
        }

        // public async Task<IActionResult> Index(string searchString)
        // {
        //     var products = _context.products.Select(p => p);

        //     if (!String.IsNullOrEmpty(searchString))
        //     {
        //         products = _context.products.Where(p => p.name.ToLower().Contains(searchString.ToLower()) || p.type.ToLower().Contains(searchString.ToLower()));
        //     }

        //     return View(await products.ToListAsync());
        // }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,type,name,description,price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,type,name,description,price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
