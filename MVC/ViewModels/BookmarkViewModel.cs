using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.ViewModels
{
    public class BookmarkViewModel
    {
        public List<Bookmark> Bookmarks {get;set;}
        public int currentProduct {get;set;}
        public int productAmount {get;set;}
    }
}