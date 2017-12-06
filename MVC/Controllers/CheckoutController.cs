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
using MVC.Models;
using MVC.Models.ManageViewModels;
using MVC.Services;
using Microsoft.EntityFrameworkCore;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public CheckoutController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
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
            };

            ViewData["Message"] = sendit(model.Email);

            return View(model);
        }

        public static string sendit(string ReciverMail)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("robomarkt.g3@gmail.com");
            msg.To.Add(ReciverMail);
            msg.Subject = "Your Robomarkt order! " + DateTime.Now.ToString();
            msg.Body = "Hi panda";
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
    }
}