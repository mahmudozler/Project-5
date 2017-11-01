using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lesson4.Models;
//using Microsoft.AspNetCore.Http;
using MVC.Extension;

namespace lesson4.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            /*Dictionary<string, string> LoggedUser = new Dictionary<string, string>();
            LoggedUser.Add("name","user1");*/

            HttpContext.Session.Set("user",new UserSession(1,"Satrya"));
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your Application description page.";
            UserSession user = HttpContext.Session.Get<UserSession>("user");
            return Content(user.Username);
            //return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
