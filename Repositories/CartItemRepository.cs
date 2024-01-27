using System.Collections.Generic;
using DurableFunctionProject.Entities;

namespace DurableFunctionProject.Repositories
{
    public class CartItemRepository<T> : BaseRepository<T>
        where T : CartItemEntity
    {
        public CartItemRepository(string tableName) : base(tableName)
        {
        }

        public IAsyncEnumerable<CartItemEntity> GetAllByCartId(string cartId)
        {
            return _tableClient.QueryAsync<T>(e => e.PartitionKey == _tableName && e.CartRowKey == cartId);
        }
    }
}
