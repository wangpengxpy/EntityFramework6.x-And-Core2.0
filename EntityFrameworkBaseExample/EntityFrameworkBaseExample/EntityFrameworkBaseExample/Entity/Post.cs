using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkBaseExample.Entity
{
    public class Post: BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}