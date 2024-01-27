using System.Collections.Generic;
using System.Linq;
using DurableFunctionProject.Services;

namespace DurableFunctionProject.Models
{
    public class CartModel : BaseModel
    {
        public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();

        public double Total
        {
            get
            {
                return Items.Sum(i => new ProductService(null).Get(i.ProductId).Price * i.Quantity);
            }
        }
    }
}
