using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using DurableFunctionProject.Services;
using DurableFunctionProject.Models;
using DurableFunctionProject.Models.Enums;

namespace DurableFunctionProject
{
    public static class DurableFunctionsOrchestration
    {
        #region Orchestration

        [FunctionName("DurableFunctionsOrchestration")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var orderInfo = context.GetInput<OrderInfoModel>();

            var cartModel = new CartModel() { Id = "" };

            foreach (var item in orderInfo.Items)
            {
                var product = new ProductModel() { Name = item.Product.Name, Price = item.Product.Price };
                product = await context.CallActivityAsync<ProductModel>("InsertProduct", product);

                cartModel.Items.Add(new CartItemModel() { ProductId = product.Id, Quantity = item.Quantity });
            }

            var cart = await context.CallActivityAsync<CartModel>("AddToCart", cartModel);

            if (cart != null)
            {
                var orderModel = new OrderModel() { CartId = cart.Id, Consumer = orderInfo.Consumer, Address = orderInfo.Address };
                var order = await context.CallActivityAsync<OrderModel>("ApproveOrder", orderModel);

                if (order != null && order.Status == EnumExtension.GetDescriptionFromValue(EOrderStatus.Approved))
                {
                    order = await context.CallActivityAsync<OrderModel>("SendOrder", order);

                    if (order != null)
                        await context.CallActivityAsync("CompleteOrder", order);
                }
            }
        }

        [FunctionName("OrderApproval")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoModel>(requestBody);

            string instanceId = await starter.StartNewAsync<OrderInfoModel>("DurableFunctionsOrchestration", orderInfo);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        #endregion

        #region Product

        [FunctionName("ListProducts")]
        public static async Task<IActionResult> ListProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var products = await new ProductService(log).GetAll();
            return new OkObjectResult(products);
        }

        [FunctionName("InsertProduct")]
        public static ProductModel ActivityInsertProduct([ActivityTrigger] ProductModel product, ILogger log)
        {
            product = new ProductService(log).Insert(product);
            return product;
        }

        [FunctionName("HttpInsertProduct")]
        public static async Task<IActionResult> InsertProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<ProductModel>(requestBody);

                product = new ProductService(log).Insert(product);

                return new OkObjectResult(product);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion

        #region Cart

        [FunctionName("AddToCart")]
        public static CartModel ActivityAddToCart([ActivityTrigger] CartModel cart, ILogger log)
        {
            cart = new CartService(log).AddToCart(cart);
            return cart;
        }

        [FunctionName("HttpAddToCart")]
        public static async Task<IActionResult> AddToCart(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var cart = JsonConvert.DeserializeObject<CartModel>(requestBody);

                var cartService = new CartService(log);
                cart = cartService.AddToCart(cart);

                return new OkObjectResult(cart);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion

        #region Order

        [FunctionName("ApproveOrder")]
        public static OrderModel ActivityApproveOrder([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            orderModel = new OrderService(log).ApproveOrder(orderModel);
            return orderModel;
        }

        [FunctionName("HttpApproveOrder")]
        public static async Task<IActionResult> ApproveOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(requestBody);

                order = new OrderService(log).ApproveOrder(order);

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("SendOrder")]
        public static OrderModel ActivitySendOrder([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            orderModel = new OrderService(log).SendOrder(orderModel);
            return orderModel;
        }

        [FunctionName("HttpSendOrder")]
        public static async Task<IActionResult> SendOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(requestBody);

                order = new OrderService(log).SendOrder(order);

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("CompleteOrder")]
        public static OrderModel ActivityCompleteOrder([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            orderModel = new OrderService(log).CompleteOrder(orderModel);
            return orderModel;
        }

        [FunctionName("HttpCompleteOrder")]
        public static async Task<IActionResult> CompleteOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(requestBody);

                order = new OrderService(log).CompleteOrder(order);

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #endregion
    }
}

//fiaporderstorage
//rg-fiaporder