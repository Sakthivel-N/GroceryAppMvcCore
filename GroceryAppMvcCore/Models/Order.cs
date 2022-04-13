using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }

		[Required]
		public int CartId { get; set; }
		public virtual Cart Cart { get; set; }

		[Required, MaxLength(20)]
		public string PaymentMode { get; set; }

		[Required, MaxLength(20)]
		[DataType(DataType.Date)]
		public String DeliveryDate { get; set; }

		[Required]
		public int TotalValue { get; set; }

	}
}
