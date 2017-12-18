using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.IO;
using MVC.Models;
using MVC.Models.ManageViewModels;
using MVC.Services;
using MVC.ViewModels;
using MVC.Data;

namespace MVC.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly ProductDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public ShoppingCartController(ShoppingCart shoppingCart, ProductDbContext context, UserManager<ApplicationUser> userManager)
        {
            _shoppingCart = shoppingCart;
            _context = context;
            _userManager = userManager;

        }

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

            if (_shoppingCart.GetShoppingCartItems().Where(p => p.Product.Id == Id).FirstOrDefault() != null)
            {
                if (_shoppingCart.GetShoppingCartItems().Where(p => p.Product.Id == Id).FirstOrDefault().Amount < selectedProduct.Amount)
                {
                    _shoppingCart.AddToCart(selectedProduct, 1);
                }
            }
            else if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
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

        public RedirectToActionResult ClearCart()
        {
            _shoppingCart.ClearCart();

            return RedirectToAction("Index");
        }

        public IActionResult QuickAddToCart(int Id)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);

            }

            return View("~/Views/Product/Index.cshtml");

        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                Address = user.Address

            };

            var orderDate = DateTime.Now;
            string orderId = getOrderId();

            foreach (var si in _shoppingCart.GetShoppingCartItems())
            {
                PartialOrder order = new PartialOrder()
                {
                    UserId = user.Id,
                    OrderId = orderId,
                    ProductId = _context.Products.Where(p => p.Id == si.Product.Id).First().Id,
                    Price = _shoppingCart.GetShoppingCartTotal(),
                    Amount = si.Amount,
                    Date = orderDate
                };

                _context.PartialOrder.Update(order);

                _context.Products.Where(p => p.Id == si.Product.Id).First().Amount -= si.Amount;
                _context.SaveChanges();
            }
            ClearCart();
            ViewData["Message"] = this.sendit(model.Email, model.Username, user.Address);

            return View(model);
        }

        public string getOrderId()
        {
            Random random = new Random();
            string orderId = "20" + random.Next(0, 100).ToString("000") + "-" +
                             DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString().Substring(2) + "-" +
                             DateTime.Now.Hour.ToString() + DateTime.Now.Millisecond.ToString();

            return orderId;
        }

        public string sendit(string ReciverMail, string username, string Adres)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("robomarkt.g3@gmail.com");
            msg.To.Add(ReciverMail);
            msg.Subject = "Your Robomarkt order! " + DateTime.Now.ToString();
            msg.Body = this.CreateBody(username, Adres);
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

        private string CreateBody(string UserName, string Adres)
        {
            string body = string.Empty;
            ShoppingCart shoppingCart = _shoppingCart;
            using (StreamReader reader = new StreamReader("../MVC/Component/EmailTemplate.html"))
            {
                body = reader.ReadToEnd();
            }

            string bodystring = "";
            foreach (var cartitem in shoppingCart.GetShoppingCartItems())
            {

                bodystring +=
                "<tr><td>" + cartitem.Product.Name +
                " </td><td>" + cartitem.Amount.ToString() +
                "</td><td>" + "€ " + cartitem.Product.Price.ToString() +
                "</td><td></td></tr>";


            }
            bodystring += "<tr><td>" +
                " </td><td>" +
                "</td><td>" +
                "</td><td>" + "€ " + shoppingCart.GetShoppingCartTotal().ToString() + "</td></tr>";

            body = body.Replace("{swapthatshit}", bodystring);
            body = body.Replace("{Username}", UserName);
            body = body.Replace("{Geen Adres}", Adres);

            return body;
        }

    }


}