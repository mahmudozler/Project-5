using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductModel;

namespace lesson4.Controllers
{
    public class SpecificationController : Controller
    {
        private readonly ProductContext _context;

        public SpecificationController(ProductContext context)
        {
            _context = context;
        }

        // GET: Specification
        public async Task<IActionResult> Index()
        {
            var productContext = _context.specifications.Include(s => s.product);
            return View(await productContext.ToListAsync());
        }

        // GET: Specification/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specification = await _context.specifications
                .Include(s => s.product)
                .SingleOrDefaultAsync(m => m.id == id);
            if (specification == null)
            {
                return NotFound();
            }

            return View(specification);
        }

        // GET: Specification/Create
        public IActionResult Create()
        {
            ViewData["productid"] = new SelectList(_context.products, "id", "id");
            return View();
        }

        // POST: Specification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,value,productid")] Specification specification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productid"] = new SelectList(_context.products, "id", "id", specification.productid);
            return View(specification);
        }

        // GET: Specification/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specification = await _context.specifications.SingleOrDefaultAsync(m => m.id == id);
            if (specification == null)
            {
                return NotFound();
            }
            ViewData["productid"] = new SelectList(_context.products, "id", "id", specification.productid);
            return View(specification);
        }

        // POST: Specification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,value,productid")] Specification specification)
        {
            if (id != specification.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecificationExists(specification.id))
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
            ViewData["productid"] = new SelectList(_context.products, "id", "id", specification.productid);
            return View(specification);
        }

        // GET: Specification/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specification = await _context.specifications
                .Include(s => s.product)
                .SingleOrDefaultAsync(m => m.id == id);
            if (specification == null)
            {
                return NotFound();
            }

            return View(specification);
        }

        // POST: Specification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specification = await _context.specifications.SingleOrDefaultAsync(m => m.id == id);
            _context.specifications.Remove(specification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecificationExists(int id)
        {
            return _context.specifications.Any(e => e.id == id);
        }
    }
}
