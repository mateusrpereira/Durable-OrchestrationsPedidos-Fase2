using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;

namespace DurableFunctionProject.Services
{
    public class ProductService : BaseService<ProductModel, ProductEntity>
    {
        public ProductService(ILogger log) : base("Product", log)
        {
        }

        public override ProductModel ConvertEntityToModel(ProductEntity entity)
        {
            return new ProductModel()
            {
                Id = entity.RowKey,
                Name = entity.Name,
                Price = entity.Price,
            };
        }

        public override ProductEntity ConvertModelToEntity(ProductModel model)
        {
            return new ProductEntity()
            {
                RowKey = model.Id,
                Name = model.Name,
                Price = model.Price,
            };
        }
    }
}
