using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkBaseExample.Entity
{
    public class Person
    {
        [Required]
        [StringLength(10, ErrorMessage = "字数不能超过10个字符")]
        public string Name { get; set; }

        [Required]
        public string EnglishName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}