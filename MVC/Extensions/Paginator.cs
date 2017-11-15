using System;
using System.Linq;

namespace MVC.Services
{
    public class Page<T>
    {
        public int index { get; set; }
        public T[] items { get; set; }
        public int totalPages { get; set; }
    }

    public static class Paginator
    {
        public static Page<T> GetPage<T>(this Microsoft.EntityFrameworkCore.DbSet<T> list, int page_index, int page_size, Func<T, object> order_by_selector, Func<T, bool> filter_by_predicate = null)
            where T : class
        {
            var res = list.OrderBy(order_by_selector)
                          .Skip(page_index * page_size)
                          .Take(page_size)
                          .ToArray();

            var tot_items = list.Count();

            if (filter_by_predicate != null)
            {
                res = list.Where(filter_by_predicate)
                          .OrderBy(order_by_selector)
                          .Skip(page_index * page_size)
                          .Take(page_size)
                          .ToArray();

                tot_items = list.Where(filter_by_predicate)
                                .Count();
            }

            if (res == null || res.Length == 0) { return null; }


            var tot_pages = tot_items / page_size;
            if (tot_items < page_size) { tot_pages = 1; }

            return new Page<T>() { index = page_index, items = res, totalPages = tot_pages };
        }
    }
}