using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart {get;set;}
        public float ShoppingCartTotal {get;set;}
    }
}