using System;

namespace EntityFramework6.Enity
{
    public class Order : BaseEntity
    {
        public int Quantity { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
