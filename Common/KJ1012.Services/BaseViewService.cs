using System.Linq;
using KJ1012.Core.Data;
using KJ1012.Core.Extensions;
using KJ1012.Core.Infrastructure;
using KJ1012.Core.User;
using KJ1012.Data.Entities;
using KJ1012.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KJ1012.Services
{
    public abstract class BaseViewService<T> : IBaseViewService<T> where T : BaseView
    {
        private readonly IViewRepository<T> _query;

        public DbSet<T> BaseView => _query.View;
        protected BaseViewService(IViewRepository<T> query)
        {
            _query = query;
            CurrentUser = EngineContext.Current.GetService<User>();
        }
        protected User CurrentUser { get; }
        public virtual IQueryable<T> GetList(SearchData searchData)
        {
            return _query.View;
        }

        public virtual (int Count, IQueryable<T> List) GetPagerList(SearchData searchData)
        {
            var query = _query.View;
            var pagerQuery = query.Pager(searchData.ToPager());
            return (pagerQuery.Count, pagerQuery.Query);
        }
        /// <summary>
        /// 判断是否演示账号
        /// </summary>
        /// <returns></returns>
        protected bool IsSpecialUser()
        {
            return CurrentUser != null && CurrentUser.SystemId ==(short)AccountSystemIdEnum.Special;
        }
    }
}
