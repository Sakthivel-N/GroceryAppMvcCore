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
    }
}
