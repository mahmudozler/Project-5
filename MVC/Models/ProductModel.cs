using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        public List<Specification> Specifications { get; set; }
    }

    public class Specification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class OrderHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set;}
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}