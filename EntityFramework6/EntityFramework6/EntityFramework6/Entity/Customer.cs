using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EntityFramework6.Enity
{
    public partial class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
