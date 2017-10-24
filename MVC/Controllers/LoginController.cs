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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string User, string Pass)
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
                    ViewData["Message"] = "You have succesfully logged in.";
                    return View();

                }
                else
                {
                    ViewData["Message"] = "Wrong password, Try again.";
                    return View();

                }
            }
            catch
            {
                ViewData["Message"] = "Couldn't find your account.";
                return View();
            }
        }

        public IActionResult Account()
        {
            return View();
        }
    }
}