using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> products { get; set; }
        public DbSet<Specification> specifications { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=Dingdong50;Host=localhost;Port=5432;Database=robomarkt;Pooling=true;");
        }
    }

    public class Product
    {
        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public List<Specification> specifications { get; set; }
    }

    public class Specification
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public int productid { get; set; }
        public Product product { get; set; }
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