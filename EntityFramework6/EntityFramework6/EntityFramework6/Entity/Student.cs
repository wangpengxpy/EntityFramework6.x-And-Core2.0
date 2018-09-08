using EntityFramework6.Enity;
using System.Collections.Generic;

namespace EntityFramework6.Entity
{
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public byte Age { get; set; }
    }
}
