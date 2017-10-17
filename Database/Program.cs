using Model;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            // DataInsertion();
            Projection();
        }

        static void DataInsertion()
        {
            using (var db = new ProductContext())
            {
                string[] file_locs = new[] { "laptops", "desktops", "printers", "monitors", "mouse", "keyboards" };
                foreach (string file_loc in file_locs)
                {
                    string[] file_row = File.ReadAllText(String.Format("Data/product_{0}.csv", file_loc)).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    List<string> headers = file_row[0].Split(',').ToList();

                    for (int i = 1; i < file_row.Length - 1; i++)
                    {
                        string[] file_column = Regex.Split(file_row[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                        if (file_column.Length == headers.Count)
                        {
                            string product_type = file_column[0].Replace("\"", "");
                            string product_name = file_column[1].Replace("\"", "");
                            string product_desc = file_column[2].Replace("\"", "");
                            float product_price = 0.00f;
                            float.TryParse(file_column[3].Replace("-", "00").Replace("\"", "").Replace(",", ";").Replace(".", ",").Replace(";", "."), out product_price);
                            List<Specification> specs = new List<Specification>();

                            for (int j = 4; j < file_column.Length; j++)
                            {
                                specs.Add(new Specification
                                {
                                    name = headers[j].Replace("\"", ""),
                                    value = file_column[j].Replace("\"", "")
                                });
                            }

                            Product p = new Product
                            {
                                type = product_type,
                                name = product_name,
                                description = product_desc,
                                price = product_price,
                                specifications = specs
                            };

                            db.Add(p);
                        }
                    }
                }

                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
            }
        }

        static void Projection()
        {
            using (var db = new ProductContext())
            {
                var projected_products = from p in db.products
                                         select new { p.id, p.type, p.name, p.price };

                Console.WriteLine("id | type | name | price");
                foreach (var product in projected_products)
                {
                    Console.WriteLine("- {0} | {1} | {2} | {3}", product.id, product.type, product.name, product.price);
                }
            }
        }
    }
}
