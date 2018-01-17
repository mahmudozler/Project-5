using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.ViewModels
{
    public class SubcribeViewModel
    {
        public List<Sub> SubsribeStatus {get;set;}
        public int currentProduct {get;set;}
        public int productAmount {get;set;}
    }
}