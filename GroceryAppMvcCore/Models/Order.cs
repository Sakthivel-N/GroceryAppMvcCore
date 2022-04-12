using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Order
    {
		[Key]
		public int OrderId { get; set; }

		[Required]
		public int ProductId { get; set; }
		public virtual Product Products { get; set; }

		[Required]
		public int UserId { get; set; }
		public virtual User Users { get; set; }

		[Required]
		public int PurchasedQty { get; set; }

		[Required, MaxLength(20)]
		public string PaymentMode { get; set; }

		[Required , MaxLength(20)]
		[DataType(DataType.Date)]
		public String DeliveryDate { get; set; }

		[Required]
		public int TotalValue { get; set; }		

	}//
}
