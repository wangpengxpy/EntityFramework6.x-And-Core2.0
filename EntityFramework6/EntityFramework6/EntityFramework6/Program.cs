using EntityFramework6.Enity;
using System;
using System.Linq;

namespace EntityFramework6
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new EfDbContext())
            {
                ctx.Database.Log = Console.WriteLine;

                var customers = ctx.Customers;

                var customer = customers.FindAsync(1).Result;
            }
            Console.ReadKey();
        }
    }
}
