using System;
using System.Linq;

namespace EFCore.Model
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Count { get; set; }
        public byte[] RowVersion { get; set; }
        public string RowVersionString =>
       $"0x{BitConverter.ToUInt64(RowVersion.Reverse().ToArray(), 0).ToString("X16")}";
    }
}
