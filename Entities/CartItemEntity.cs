namespace DurableFunctionProject.Entities
{
    public class CartItemEntity : BaseEntity
    {
        public string CartRowKey { get; set; }
        public string ProductRowKey { get; set; }
        public int Quantity { get; set; }
    }
}
