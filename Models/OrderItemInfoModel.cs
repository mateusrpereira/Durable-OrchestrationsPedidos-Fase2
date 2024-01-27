using System.Collections.Generic;

namespace DurableFunctionProject.Models
{
    public class OrderItemInfoModel
    {
        public ProductModel Product { get; set; } = new ProductModel();
        public int Quantity { get; set; }
    }
}
