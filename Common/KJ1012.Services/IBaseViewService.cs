using System.Linq;
using KJ1012.Core.Data;
using KJ1012.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KJ1012.Services
{
    public interface IBaseViewService<T>: IDataService where T : BaseView
    {
        DbSet<T> BaseView { get; }
        IQueryable<T> GetList(SearchData searchData);

        (int Count, IQueryable<T> List) GetPagerList(SearchData searchData);
    }
}
