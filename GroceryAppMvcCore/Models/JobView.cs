namespace GroceryAppMvcCore.Models
{
    public class JobView
    {
        public List<Delivery> deliveries { get; set; }
        public List<Order> orders { get; set; }
        public List<Cart> carts { get; set; }
        public List<Product> products { get; set; }

        public List<User> users { get; set; }
    }
}
