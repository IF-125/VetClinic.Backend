using System.Collections.Generic;
using System.Threading.Tasks;

namespace VetClinic.Core.Interfaces.Services.Base
{
    public interface IBaseService<TEntity, TIdType>
        where TEntity : class
    {
        public Task<TEntity> GetByIdAsync(TIdType id);

        public Task InsertAsync(TEntity entity);

        public void Update(TIdType id, TEntity entityToUpdate);

        public Task DeleteAsync(TIdType id);

        public Task DeleteRangeAsync(IList<TIdType> listOfIds);
    }
}