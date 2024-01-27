using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;
using DurableFunctionProject.Repositories;

namespace DurableFunctionProject.Services
{
    public class OrderItemService : BaseService<OrderItemModel, OrderItemEntity>
    {
        private const string _tableName = "OrderItem";

        public OrderItemService(ILogger log) : base(_tableName, log)
        {
        }

        public override OrderItemModel ConvertEntityToModel(OrderItemEntity entity)
        {
            return new OrderItemModel()
            {
                Id = entity.RowKey,
                OrderId = entity.OrderRowKey,
                ProductId = entity.ProductRowKey,
                Quantity = entity.Quantity,
            };
        }

        public override OrderItemEntity ConvertModelToEntity(OrderItemModel model)
        {
            return new OrderItemEntity()
            {
                RowKey = model.Id,
                OrderRowKey = model.OrderId,
                ProductRowKey = model.ProductId,
                Quantity = model.Quantity,
            };
        }

        public async Task<IList<OrderItemModel>> GetAllByOrderId(string orderId)
        {
            var entities = new OrderItemRepository<OrderItemEntity>(_tableName).GetAllByOrderId(orderId);

            IList<OrderItemModel> list = new List<OrderItemModel>();

            await foreach (OrderItemEntity entity in entities)
                list.Add(ConvertEntityToModel(entity));

            return list;
        }
    }
}
