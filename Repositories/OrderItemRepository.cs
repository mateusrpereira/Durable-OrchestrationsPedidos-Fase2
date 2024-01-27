using System.Collections.Generic;
using DurableFunctionProject.Entities;

namespace DurableFunctionProject.Repositories
{
    public class OrderItemRepository<T> : BaseRepository<T>
        where T : OrderItemEntity
    {
        public OrderItemRepository(string tableName) : base(tableName)
        {
        }

        public IAsyncEnumerable<OrderItemEntity> GetAllByOrderId(string orderId)
        {
            return _tableClient.QueryAsync<T>(e => e.PartitionKey == _tableName && e.OrderRowKey == orderId);
        }
    }
}
