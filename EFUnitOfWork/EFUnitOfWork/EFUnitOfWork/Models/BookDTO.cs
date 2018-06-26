using System;

namespace EFUnitOfWork.Models
{
    public class BookDTO
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime Published { get; set; }
        public string IP { get; set; }
        public string Url { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}