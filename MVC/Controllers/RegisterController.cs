using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductModel;

namespace lesson4.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ProductContext _context;

        public RegisterController(ProductContext context)
        {
            _context = context;
        }

        static string GetSalt()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 5)
                                .Select(s => s[new Random().Next(s.Length)]).ToArray());
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

        public IActionResult Register(string User, string Pass, string Mail)
        {
            var buffer = from u in _context.users
                         select new { u.username, u.email };

            List<string> usernames = new List<string>();
            List<string> emails = new List<string>();

            foreach (var user in buffer)
            {
                usernames.Add(user.username);
                emails.Add(user.email);
            }

            if (usernames.Contains(User))
            {
                ViewData["Message"] = "That username is taken. Try another.";
                return View();
            }
            else if (emails.Contains(Mail))
            {
                ViewData["Message"] = "That email is taken. Try another.";
                return View();
            }
            else
            {
                string Salt = GetSalt();
                string Password = GetSHA512Hash(Pass, Salt);

                User newUser = new User()
                {
                    username = User,
                    password = Password,
                    salt = Salt,
                    email = Mail
                };

                _context.users.Add(newUser);
                _context.SaveChanges();

                ViewData["Message"] = "You have succesfully registered.";
                return View();
            }
        }
    }
}
