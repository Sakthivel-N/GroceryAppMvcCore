using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(20)]
        public string CategoryName { get; set; }


    }
}
