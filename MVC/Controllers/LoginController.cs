using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductModel;
using Microsoft.AspNetCore.Http;
using MVC.Extension;

namespace lesson4.Controllers
{
    public class LoginController : Controller
    {
        private readonly ProductContext _context;

        public LoginController(ProductContext context)
        {
            _context = context;
        }

        static string GetSHA512Hash(string input, string salt = "")
        {
            SHA512 shaM = SHA512.Create();
            StringBuilder sBuilder = new StringBuilder();

            byte[] data = shaM.ComputeHash(Encoding.ASCII.GetBytes(input + salt));

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        
        [HttpGet]
        public IActionResult Index(string message)
        {
            if(HttpContext.Session.GetString("User") != null){
                return RedirectToAction("Account");
            }

            TempData["Message"] = message;
            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(string User, string Pass)
        {
            var buffer = from u in _context.users
                         select new { u.username, u.password, u.salt };

            List<string> usernames = new List<string>();
            List<string> passwords = new List<string>();
            List<string> salts = new List<string>();

            foreach (var user in buffer)
            {
                usernames.Add(user.username);
                passwords.Add(user.password);
                salts.Add(user.salt);
            }

            try
            {
                int index = usernames.IndexOf(User);

                if (passwords[index] == GetSHA512Hash(Pass, salts[index]))
                {
                    HttpContext.Session.Set("User", new UserSession(1, User));
                    return RedirectToAction("Account");
                }
                else
                {
                    return Index("Wrong password, Try again.");
                }
            }
            catch
            {
                return Index("Couldn't find your account.");
            }
        }

        public IActionResult Account(string userName)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }
            
            ViewData["username"] = HttpContext.Session.Get<UserSession>("User").Username;
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index");
        }
    }
}