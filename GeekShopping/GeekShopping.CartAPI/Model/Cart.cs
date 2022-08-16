namespace GeekShopping.CartAPI.Model
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; } = new CartHeader();

        public IEnumerable<CartDetail>? CartDetails { get; set; }
    }
}
