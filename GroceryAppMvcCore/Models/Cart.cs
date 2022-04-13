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

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int PurchasedQty { get; set; }

        [Required]
        public int ProductPrice { get; set; }


        
        public int TotalValue { get; set; }

        [Required]
        public bool IsOrdered { get; set; }

    }
}
