namespace GroceryAppMvcCore.Models
{
    public class DeliveryVuew
    {
        public List<Order> Orders { get; set; }
       
        public List<Delivery> Deliveries { get; set; }

        public List<User> Users { get; set; }
        public List<Product> Products { get; set; }

        public List<Cart> Carts { get; set; }
    }
}
