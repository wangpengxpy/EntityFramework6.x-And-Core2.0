using EntityFramework6.Enity;
using System.Collections.Generic;

namespace EntityFramework6.Entity
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int MaximumStrength { get; set; }
    }
}
