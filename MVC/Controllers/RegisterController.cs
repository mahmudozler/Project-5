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

        [HttpGet]
        public IActionResult Index(string message)
        {
            TempData["Message"] = message;

            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(string User, string Pass, string Mail, string Name, string Surname, string Gender, string Phonenumber, string Zipcode, string Address)
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
                return Index("That username is taken. Try another.");
            }
            else if (emails.Contains(Mail))
            {
                return Index("That email is taken. Try another.");
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
                    email = Mail,
                    name = Name,
                    surname = Surname,
                    gender = Gender,
                    phonenumber = Phonenumber,
                    zipcode = Zipcode,
                    address = Address
                };

                _context.users.Add(newUser);
                _context.SaveChanges();

                return View("Account", "LoginController");
            }
        }
    }
}
