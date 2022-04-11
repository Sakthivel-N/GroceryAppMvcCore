using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual UserId UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual ProductId ProductId { get; set; }
    }
}
