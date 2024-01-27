using DurableFunctionProject.Services;

namespace DurableFunctionProject.Models
{
    public class CartItemModel : BaseModel
    {
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Total
        {
            get
            {
                return new ProductService(null).Get(ProductId).Price * Quantity;
            }
        }
    }
}
