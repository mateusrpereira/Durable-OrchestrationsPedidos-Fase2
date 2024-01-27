using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableFunctionProject.Entities;
using DurableFunctionProject.Models;
using DurableFunctionProject.Repositories;
using System;

namespace DurableFunctionProject.Services
{
    public abstract class BaseService<TModel, TEntity>
        where TModel : BaseModel
        where TEntity : BaseEntity
    {
        protected ILogger _log;
        protected BaseRepository<TEntity> _baseRepository;

        protected BaseService(string tableName, ILogger log)
        {
            _baseRepository = new BaseRepository<TEntity>(tableName);
            _log = log;
        }

        public async Task<IList<TModel>> GetAll()
        {
            var entities = _baseRepository.GetAll();

            IList<TModel> list = new List<TModel>();

            await foreach (TEntity entity in entities)
                list.Add(ConvertEntityToModel(entity));

            return list;
        }

        public TModel Get(string id)
        {
            var entity = _baseRepository.Get(id);
            return ConvertEntityToModel(entity);
        }

        public TModel Insert(TModel model)
        {
            var entity = ConvertModelToEntity(model);
            entity = _baseRepository.Insert(entity);
            _log.LogInformation($"[{DateTime.Now}][{_baseRepository.TableName} created: {entity.RowKey}]");
            return ConvertEntityToModel(entity);
        }

        public void Update(TModel model)
        {
            var entity = ConvertModelToEntity(model);
            _baseRepository.Update(entity);
            _log.LogInformation($"[{DateTime.Now}][{_baseRepository.TableName} updated: {entity.RowKey}]");
        }

        public abstract TModel ConvertEntityToModel(TEntity entity);
        public abstract TEntity ConvertModelToEntity(TModel model);
    }
}
