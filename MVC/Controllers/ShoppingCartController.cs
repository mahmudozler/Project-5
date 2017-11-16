using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.ViewModels;
using MVC.Data;
using System.Data.Entity;

namespace MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly ProductDbContext _context;

        public ShoppingCartController(ShoppingCart shoppingCart,ProductDbContext context)
        {
            _shoppingCart = shoppingCart;
            _context = context;
        }

        /*public IActionResult Index()
        {
            return View();
        }*/

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                    ShoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int Id)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == Id);
            if(selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct,1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int Id)
        {
            var selectedDrink = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (selectedDrink != null)
            {
                _shoppingCart.RemoveFromCart(selectedDrink);
            }
            return RedirectToAction("Index");
        }
    }


}