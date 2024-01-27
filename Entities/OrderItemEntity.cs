namespace DurableFunctionProject.Entities
{
    public class OrderItemEntity : BaseEntity
    {
        public string OrderRowKey { get; set; }
        public string ProductRowKey { get; set; }
        public int Quantity { get; set; }
    }
}
