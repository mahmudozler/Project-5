using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.ViewModels;
using MVC.Data;

namespace MVC.Components
{
    public class BookmarkComponent: ViewComponent
    {
        private readonly ProductDbContext _context;

        public BookmarkComponent(ProductDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int productId,string userId, int amount)
        {
            var bookmarks = _context.Bookmarks.Where(b => b.UserId == userId && b.ProductId == productId).ToList();

            var bookmarkstatus = new BookmarkViewModel{
                Bookmarks = bookmarks,
                currentProduct = productId,
                productAmount = amount
            };

            return View(bookmarkstatus);
        }

    }
}