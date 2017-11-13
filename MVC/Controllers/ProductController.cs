using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using ProductModel;
using Paginator;

namespace lesson4.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        // GET: Product
        public IActionResult Index(string searchString, int pageIndex = 1)
        {
            var res = _context.products.GetPage<Product>(pageIndex - 1, 25, p => p.id);

            ViewBag.searchString = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                res = _context.products.GetPage<Product>(pageIndex - 1, 25, p => p.id,
                                                                                p => p.name.ToLower().Contains(searchString.ToLower()) ||
                                                                                p.type.ToLower().Contains(searchString.ToLower()));
                ViewBag.searchString = "&searchString=" + searchString;
            }

            if (res == null) { return NotFound(); }

            ViewBag.pageIndex = pageIndex;
            ViewBag.totalPages = res.totalPages + 1;

            return View(res.items.ToList());
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

            var product = await _context.products
                .SingleOrDefaultAsync(m => m.id == id);
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

            var product = await _context.products.SingleOrDefaultAsync(m => m.id == id);
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
            if (id != product.id)
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
                    if (!ProductExists(product.id))
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

            var product = await _context.products
                .SingleOrDefaultAsync(m => m.id == id);
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
            var product = await _context.products.SingleOrDefaultAsync(m => m.id == id);
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.products.Any(e => e.id == id);
        }
    }
}
