using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.ViewModels
{
    public class ManageUserViewModel
    {
        public ApplicationUser user {get;set;}
        public IList<string> Roles {get;set;}
    }
}