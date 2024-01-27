using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;
using DurableFunctionProject.Entities;

namespace DurableFunctionProject.Repositories
{
    public class BaseRepository<T>
        where T : BaseEntity //class, ITableEntity
    {
        protected string _tableName;
        protected TableClient _tableClient;

        public string TableName
        {
            get { return _tableName; }
        }

        public BaseRepository(string tableName)
        {
            _tableName = tableName;

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            var serviceClient = new TableServiceClient(connectionString);

            _tableClient = serviceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists();
        }

        public AsyncPageable<T> GetAll()
        {
            return _tableClient.QueryAsync<T>(filter: "");
        }

        public T Get(string rowKey)
        {
            return _tableClient.Query<T>(e => e.PartitionKey == _tableName && e.RowKey == rowKey).FirstOrDefault();
        }

        public T Insert(T entity)
        {
            entity.PartitionKey = _tableName;
            entity.RowKey = Guid.NewGuid().ToString();

            _tableClient.AddEntity(entity);

            return entity;
        }

        public void Update(T entity)
        {
            entity.PartitionKey = _tableName;
            _tableClient.UpsertEntity(entity);
        }
    }
}
