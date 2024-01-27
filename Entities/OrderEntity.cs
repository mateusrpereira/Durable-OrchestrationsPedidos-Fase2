using DurableFunctionProject.Models.Enums;

namespace DurableFunctionProject.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string Consumer { get; set; }
        public string Address { get; set; }
        public EOrderStatus Status { get; set; } = EOrderStatus.Pending;
        public string CartId { get; set; }
    }
}
