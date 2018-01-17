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
    public class SubscribeComponent: ViewComponent
    {
        private readonly ProductDbContext _context;

        public SubscribeComponent(ProductDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int productId,string userId, int amount)
        {
            var subscribeStatus = _context.Subscriptions.Where(s => s.UserId == userId && s.ProductId == productId).ToList();


            var substatus = new SubcribeViewModel{
                SubsribeStatus = subscribeStatus,
                currentProduct = productId,
                productAmount = amount
            };

            return View(substatus);
        }

    }
}