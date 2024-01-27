using System.Linq;
using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;
using Microsoft.VisualBasic;
using System;

namespace DurableFunctionProject.Services
{
    public class CartService : BaseService<CartModel, CartEntity>
    {
        public CartService(ILogger log) : base("Cart", log)
        {
        }

        public override CartModel ConvertEntityToModel(CartEntity entity)
        {
            return new CartModel()
            {
                Id = entity.RowKey,
            };
        }

        public override CartEntity ConvertModelToEntity(CartModel model)
        {
            return new CartEntity()
            {
                RowKey = model.Id,
            };
        }

        public new CartModel Get(string id)
        {
            var cartEntity = _baseRepository.Get(id);
            
            if (cartEntity == null)
                return null;

            var cartModel = ConvertEntityToModel(cartEntity);

            var cartItemService = new CartItemService(_log);
            cartModel.Items.AddRange(cartItemService.GetAllByCartId(cartEntity.RowKey).Result);

            return cartModel;
        }

        public CartModel AddToCart(CartModel cart)
        {
            var cartItemService = new CartItemService(_log);

            // Cria o carrinho se não existir, caso contrário busca as informações
            var dbCart = string.IsNullOrEmpty(cart.Id) ? Insert(cart) : Get(cart.Id);

            foreach (var cartItem in cart.Items)
            {
                cartItem.CartId = dbCart.Id;

                // Inclui apenas os novos itens ao carrinho
                if (string.IsNullOrEmpty(cartItem.Id))
                {
                    // Se o produto já existir no carrinho
                    var dbCartItem = dbCart.Items.FirstOrDefault(i => i.ProductId == cartItem.ProductId);
                    if (dbCartItem != null)
                    {   // apenas modifica a quantidade
                        dbCartItem.Quantity += cartItem.Quantity;
                        cartItemService.Update(dbCartItem);
                    }
                    else
                    {   // inclui o produto no carrinho
                        dbCartItem = cartItemService.Insert(cartItem);
                        dbCart.Items.Add(dbCartItem);
                    }

                    _log.LogInformation($"[{DateTime.Now}][Item added to cart: {cartItem.ProductId}, Quantity: {cartItem.Quantity}, Total do Item: {cartItem.Total}]");
                }
            }

            return dbCart;

        }
    }
}
