namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class CartVO
    {
        public CartHeaderVO CartHeader { get; set; } = new CartHeaderVO();

        public IEnumerable<CartDetailVO>? CartDetails { get; set; }
    }
}
