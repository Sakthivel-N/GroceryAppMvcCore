using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductPrice { get; set; }


        [Required]
        public int PurchasedQty { get; set; }

        [Required]
        public int SubTotalPrice { get; set; }

        [Required]
        public bool IsOrdered { get; set; }

        public Product Products { get; set; }

    }
}
