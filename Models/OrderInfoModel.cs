using System.Collections.Generic;

namespace DurableFunctionProject.Models
{
    public class OrderInfoModel
    {
        public string Consumer { get; set; }
        public string Address { get; set; }
        public List<OrderItemInfoModel> Items { get; set; } = new List<OrderItemInfoModel>();
    }
}
