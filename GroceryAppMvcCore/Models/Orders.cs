using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Orders
    {
		[Key]
		public int OrderId { get; set; }

		[Required, MaxLength(15)]
		public int ProductId { get; set; }
		public virtual products Products { get; set; }

		[Required]
		public int UserId { get; set; }
		public virtual users users { get; set; }

		[Required]
		public int PurchasedQty { get; set; }

		[Required, MaxLength(40)]
		public string PaymentMode { get; set; }

		[Required , MaxLength(40)]
		[DataType(DataType.Date)]
		public String DeliveryDate { get; set; }

		[Required]
		public int TotalValue { get; set; }		

	}
}
