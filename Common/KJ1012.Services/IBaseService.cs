using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Core.Data;
using KJ1012.Data.Entities;

namespace KJ1012.Services
{
    public interface IBaseService<T>: IDataService where T : BaseEntity
    {
        IRepository<T> BaseRepository { get;}
        Task<T> GetEntityByKey(params object[] id);
        IQueryable<T> GetList(SearchData searchData);
        IQueryable<T> GetList();
        IQueryable<T> GetListByTrack();
        (int Count, IQueryable<T> List) GetPagerList(SearchData searchData, DataOperation operation=null);

        Task<int> SaveAsync(T entity, bool alwaysUpdate = false);
        Task<int> DeleteAsync(T entity);
        Task<int> DeleteAsync(IEnumerable<T> entity);
    }
}
