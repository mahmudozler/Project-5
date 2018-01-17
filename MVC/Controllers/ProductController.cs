using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MVC.Models;
using MVC.Data;
using MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MVC.Controllers
{
    //[Authorize] [Authorize("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ProductDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Product
        public IActionResult Index(string searchString, int pageIndex = 1, string orderBy = "Best")
        {
            ViewData["Message"] = "Alle Producten";

            Func<Product, object> order_by_selector;
            bool descending = false;

            switch (orderBy)
            {
                case "Best":
                    updateSold();
                    order_by_selector = p => p.Sold;
                    descending = true;
                    ViewBag.orderByString = "Best Verkocht";
                    break;
                case "PrijsLH":
                    order_by_selector = p => p.Price;
                    ViewBag.orderByString = "Prijs Laag - Hoog";
                    break;
                case "PrijsHL":
                    order_by_selector = p => p.Price;
                    descending = true;
                    ViewBag.orderByString = "Prijs Hoog - Laag";
                    break;
                default:
                    order_by_selector = p => p.Name;
                    ViewBag.orderByString = "Alphabetisch";
                    break;
            }

            var res = _context.Products.GetPage<Product>(pageIndex - 1, 24, order_by_selector);

            if (descending == true)
            {
                res = _context.Products.GetPage<Product>(pageIndex - 1, 24, order_by_selector, descending);
            }

            ViewBag.searchString = "";
            ViewBag.orderBy = "&orderBy=" + orderBy;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (descending == true)
                {
                    res = _context.Products.GetPage<Product>(pageIndex - 1, 24, order_by_selector, descending,
                                                                                p => p.Name.ToLower().Contains(searchString.ToLower()) ||
                                                                                p.Type.ToLower().Contains(searchString.ToLower()));
                }
                else
                {
                    res = _context.Products.GetPage<Product>(pageIndex - 1, 24, order_by_selector,
                                                                                    filter_by_predicate:
                                                                                    p => p.Name.ToLower().Contains(searchString.ToLower()) ||
                                                                                    p.Type.ToLower().Contains(searchString.ToLower()));
                }
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

            var product_specifications = from p in _context.Products
                                         from s in _context.Specifications
                                         where p.Id == id
                                         where p.Id == s.ProductId
                                         select new Specification
                                         {
                                             Name = s.Name,
                                             Value = s.Value
                                         };

            product.Specifications = product_specifications.ToList();

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
        public async Task<IActionResult> Create([Bind("Id,Type,Name,Description,Price,Amount")] Product product)
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
        public async Task<IActionResult> Edit(int id, int OriginalAmount,[Bind("Id,Type,Name,Description,Price,Amount")] Product product)

        {

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //product amount on 0 and amount updated to >0
                    if(OriginalAmount <= 0 && product.Amount > 0){
                        var subscribers = _context.Subscriptions.Where(p => p.ProductId == id).ToList();
                        var emailList = new List<string>();
                        foreach(var sub in subscribers){
                            var u = await _userManager.FindByIdAsync(sub.UserId.ToString());
                            emailList.Add(u.UserName);
                        }
                        sendit(emailList,product.Name,product.Id); 
                        foreach(var sub in subscribers){
                            _context.Subscriptions.Remove(sub);
                        }
                    }

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

        private void updateSold()
        {
            foreach (var p in _context.Products)
            {
                p.Sold = 0;
            }

            DateTime today = DateTime.Now.Date;

            foreach (var order in _context.PartialOrder.Where(p => (today - p.Date).TotalDays <= 7))
            {
                var temp_product = _context.Products.Where(p => p.Id == order.ProductId).FirstOrDefault();

                temp_product.Sold = order.Amount;
            }

            _context.SaveChanges();
        }

        public async Task<IActionResult> Bookmark(int productId){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _context.Bookmarks.Add(new Bookmark(){ 
                ProductId = productId,
                UserId = user.Id
             });

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new{ id = productId});
        }

        public async Task<IActionResult> RemoveBookmark(int productId){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var bookmarkrem = _context.Bookmarks.Where(b => b.UserId == user.Id && b.ProductId == productId).FirstOrDefault();

            _context.Bookmarks.Remove(bookmarkrem);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new{ id = productId});
        }

        public async Task<IActionResult> RemoveBookmarkFromlist(int productId){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var bookmarkrem = _context.Bookmarks.Where(b => b.UserId == user.Id && b.ProductId == productId).FirstOrDefault();

            _context.Bookmarks.Remove(bookmarkrem);

            await _context.SaveChangesAsync();
            return 	RedirectToAction("Bookmarks","Manage");
        }

        public async Task<IActionResult> Subscribe(int productId){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _context.Subscriptions.Add(new Sub(){ 
                ProductId = productId,
                UserId = user.Id
             });

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new{ id = productId});
        }

        public async Task<IActionResult> UnSubscribe(int productId){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var subrem = _context.Subscriptions.Where(s => s.UserId == user.Id && s.ProductId == productId).FirstOrDefault();
            
            _context.Subscriptions.Remove(subrem);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new{ id = productId});
        }

        public string sendit(List<string> mailList,string productName,int productId)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("robomarkt.g3@gmail.com");
            foreach(var email in mailList){
                msg.To.Add(email);
            }
            msg.Subject = "Robomarkt - " + productName + " is weer op voorraad! " + DateTime.Now.ToString();
            msg.Body = this.CreateBody(productName,productId);
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("robomarkt.g3@gmail.com", "MachineLearning");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return " ";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }

        private string CreateBody(string productName, int productId)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("../MVC/Component/EmailUpdateStock.cshtml"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{productName}", productName);
            body = body.Replace("{productId}", productId.ToString());

            return body;
        }

        

    }
}
