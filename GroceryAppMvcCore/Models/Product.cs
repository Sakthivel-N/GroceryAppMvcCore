using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }
        [Required, MaxLength(20)]
        public string ProductDetails { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required, MaxLength(50)]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
