namespace ProjectLapShop.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            lstItems=new List<ShoppingCartItem>();
        }
        public List<ShoppingCartItem> lstItems { get; set; }
        public decimal TotalCard { get; set; }
        public string PromoCode {  get; set; }
    }
}
