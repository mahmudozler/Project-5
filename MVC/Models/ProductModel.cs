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
        public int Sold { get; set; }
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

    public class PartialOrder
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class OrderHistory
    {
        public string OrderId { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public List<Product> Orders { get; set; }
    }

    public class UserOrders
    {
        public List<OrderHistory> Orders { get; set; }
    }

    public class Bookmark
    {
        public int BookmarkId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}