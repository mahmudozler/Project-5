using System;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Models
{
	public class ProductContext : DbContext
	{
		public DbSet<Product> products { get; set; }

		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
	}

	public class Product
	{
		public string description { get; set; }
		public int id { get; set; }
		public string name { get; set; }
		public Single price { get; set; }
		public string type { get; set; }
	}
}
