using System;
namespace EF.Core
{
    public abstract class BaseEntity
    {
      public Int64 ID { get; set; }
      public DateTime CreatedTime { get; set; }
      public DateTime ModifiedTime { get; set; }
      public string IP { get; set; }
    }
}
