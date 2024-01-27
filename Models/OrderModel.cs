using System.Collections.Generic;
using System.Linq;
using DurableFunctionProject.Models.Enums;

namespace DurableFunctionProject.Models
{
    public class OrderModel : BaseModel
    {
        private EOrderStatus _status { get; set; } = EOrderStatus.Pending;
        public string Consumer { get; set; }
        public string Address { get; set; }
        public string CartId { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();

        public string Status {
            get
            {
                return EnumExtension.GetDescriptionFromValue(_status);
            }
            set
            {
                _status = EnumExtension.GetValueFromDescription<EOrderStatus>(value);
            }
        }

        public double Total
        {
            get
            {
                return Items.Sum(i => i.Total);
            }
        }
    }
}
