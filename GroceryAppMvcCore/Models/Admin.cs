using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required,MinLength(5),MaxLength(20)]
        public string AdminName { get; set; }
        [Required, MinLength(5), MaxLength(20)]
        public string EmailId { get; set; }
        [Required, MinLength(5), MaxLength(20)]
        public string Password { get; set; }
        

    }
}
