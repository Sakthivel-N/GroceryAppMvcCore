namespace GroceryAppMvcCore.Models
{
    public class OrderView
    {
        public List<Order> Orders  { get; set; }
        public List<Cart> Carts { get; set; }

        public List<Delivery> Deliverys { get; set; }
        public List<Product> Products { get; set; }
        public List<User> Users { get; set; }
    }
}
