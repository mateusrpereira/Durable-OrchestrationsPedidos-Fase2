using System;
using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;
using DurableFunctionProject.Models.Enums;

namespace DurableFunctionProject.Services
{
    public class OrderService : BaseService<OrderModel, OrderEntity>
    {
        public OrderService(ILogger log) : base("Order", log)
        {
        }

        public override OrderModel ConvertEntityToModel(OrderEntity entity)
        {
            return new OrderModel()
            {
                Id = entity.RowKey,
                Status = EnumExtension.GetDescriptionFromValue(entity.Status),
                Consumer = entity.Consumer,
                Address = entity.Address,
                CartId = entity.CartId,
            };
        }

        public override OrderEntity ConvertModelToEntity(OrderModel model)
        {
            return new OrderEntity()
            {
                RowKey = model.Id,
                Status = EnumExtension.GetValueFromDescription<EOrderStatus>(model.Status),
                Consumer = model.Consumer,
                Address = model.Address,
                CartId= model.CartId,
            };
        }

        public new OrderModel Get(string id)
        {
            var orderEntity = _baseRepository.Get(id);

            if (orderEntity == null)
                return null;

            var orderModel = ConvertEntityToModel(orderEntity);

            var orderItemService = new OrderItemService(_log);
            orderModel.Items.AddRange(orderItemService.GetAllByOrderId(orderEntity.RowKey).Result);

            return orderModel;
        }

        public OrderModel ApproveOrder(OrderModel orderModel)
        {
            if (orderModel.Status != EnumExtension.GetDescriptionFromValue(EOrderStatus.Pending))
                throw new Exception("Order not approved.");

            var cart = new CartService(_log).Get(orderModel.CartId);

            if (cart.Total < 1000)
                orderModel.Status = EnumExtension.GetDescriptionFromValue(EOrderStatus.Approved);

            var order = Insert(orderModel);
            
            var ordemItemService = new OrderItemService(_log);

            foreach (var cartItem in cart.Items)
            {
                var orderItemModel = new OrderItemModel()
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                };

                orderItemModel = ordemItemService.Insert(orderItemModel);
                _log.LogInformation($"[{DateTime.Now}][Item added to order: {orderItemModel.ProductId}, Quantity: {orderItemModel.Quantity}, Total: {orderItemModel.Total}]");

                order.Items.Add(orderItemModel);
            }

            if (orderModel.Status == EnumExtension.GetDescriptionFromValue(EOrderStatus.Approved))
                _log.LogInformation($"[{DateTime.Now}][Order: {order.Id} approved successfully. Consumer: {order.Consumer}, Address: {order.Address}, Total: {order.Total}]");
            else
                _log.LogInformation($"[{DateTime.Now}][Pending approval for orders over 1,000. Wait for contact. Order: {order.Id}. Consumer: {order.Consumer}, Address: {order.Address}, Total: {order.Total}]");

            return order;
        }

        public OrderModel SendOrder(OrderModel orderModel)
        {
            var dbOrder = Get(orderModel.Id);

            if (dbOrder == null)
                throw new Exception($"Invalid Order: {orderModel.Id}");

            if (dbOrder.Status != EnumExtension.GetDescriptionFromValue(EOrderStatus.Approved))
                throw new Exception($"Order not approved: {orderModel.Id}");

            dbOrder.Status = EnumExtension.GetDescriptionFromValue(EOrderStatus.Sent);
            Update(dbOrder);

            _log.LogInformation($"[{DateTime.Now}][Order {dbOrder.Id} sent successfully.]");

            return dbOrder;
        }

        public OrderModel CompleteOrder(OrderModel orderModel)
        {
            var dbOrder = Get(orderModel.Id);

            if (dbOrder == null)
                throw new Exception($"Invalid Order: {orderModel.Id}");

            if (dbOrder.Status != EnumExtension.GetDescriptionFromValue(EOrderStatus.Sent))
                throw new Exception($"Order not approved: {orderModel.Id}");

            dbOrder.Status = EnumExtension.GetDescriptionFromValue(EOrderStatus.Delivered);
            Update(dbOrder);

            _log.LogInformation($"[{DateTime.Now}][Order {dbOrder.Id} completed successfully.]");

            return dbOrder;
        }

    }
}
