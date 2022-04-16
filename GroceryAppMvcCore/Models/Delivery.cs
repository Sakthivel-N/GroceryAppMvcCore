using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
