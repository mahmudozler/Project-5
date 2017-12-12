using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=admin1399;Host=145.24.222.165;Port=5432;Database=robomarkt;Pooling=true;");  
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

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

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string gender { get; set; }
        public string phonenumber { get; set; }
        public string zipcode { get; set; }
        public string address { get; set; }
        public bool verified { get; set; }
        public bool admin { get; set; }
    }
}