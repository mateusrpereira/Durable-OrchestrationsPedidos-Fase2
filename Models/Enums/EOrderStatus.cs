using System.ComponentModel;

namespace DurableFunctionProject.Models.Enums
{
    public enum EOrderStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Sent")]
        Sent = 3,
        [Description("Delivered")]
        Delivered = 4
    }

}
