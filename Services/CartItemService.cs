using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;
using DurableFunctionProject.Repositories;

namespace DurableFunctionProject.Services
{
    public class CartItemService : BaseService<CartItemModel, CartItemEntity>
    {
        private const string _tableName = "CartItem";

        public CartItemService(ILogger log) : base(_tableName, log)
        {
        }

        public override CartItemModel ConvertEntityToModel(CartItemEntity entity)
        {
            return new CartItemModel()
            {
                Id = entity.RowKey,
                CartId = entity.CartRowKey,
                ProductId = entity.ProductRowKey,
                Quantity = entity.Quantity,
            };
        }

        public override CartItemEntity ConvertModelToEntity(CartItemModel model)
        {
            return new CartItemEntity()
            {
                RowKey = model.Id,
                CartRowKey = model.CartId,
                ProductRowKey = model.ProductId,
                Quantity = model.Quantity,
            };
        }

        public async Task<IList<CartItemModel>> GetAllByCartId(string cartId)
        {
            var entities = new CartItemRepository<CartItemEntity>(_tableName).GetAllByCartId(cartId);

            IList<CartItemModel> list = new List<CartItemModel>();

            await foreach (CartItemEntity entity in entities)
                list.Add(ConvertEntityToModel(entity));

            return list;
        }
    }
}
