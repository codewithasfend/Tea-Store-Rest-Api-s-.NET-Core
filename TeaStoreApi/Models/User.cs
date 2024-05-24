namespace TeaStoreApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Users"; 

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
