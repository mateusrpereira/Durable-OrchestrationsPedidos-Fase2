using DurableFunctionProject.Entities;

namespace DurableFunctionProject.Repositories
{
    public class ProductRepository<T> : BaseRepository<T>
        where T : ProductEntity
    {
        public ProductRepository(string tableName) : base(tableName)
        {
        }
    }
}
