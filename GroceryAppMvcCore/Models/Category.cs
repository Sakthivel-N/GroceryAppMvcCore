using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }

        [Required,MaxLength(15)]
		public string CategoryName { get; set; }

    }
}
