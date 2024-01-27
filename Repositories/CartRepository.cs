using DurableFunctionProject.Entities;

namespace DurableFunctionProject.Repositories
{
    public class CartRepository<T> : BaseRepository<T>
        where T : CartEntity
    {
        public CartRepository(string tableName) : base(tableName)
        {
        }
    }
}
