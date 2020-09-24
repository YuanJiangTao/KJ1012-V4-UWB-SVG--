using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Core.Data;
using KJ1012.Core.Extensions;
using KJ1012.Core.Infrastructure;
using KJ1012.Core.User;
using KJ1012.Data.Entities;

namespace KJ1012.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IUnitOfWork _unitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BaseRepository = _unitOfWork.Repository<T>();
            CurrentUser = EngineContext.Current.GetService<User>();
        }
        protected User CurrentUser { get; }
        public IRepository<T> BaseRepository { get; }

        public virtual async Task<T> GetEntityByKey(params object[] id)
        {
            return await BaseRepository.GetByKeys(id);
        }

        public virtual IQueryable<T> GetList(SearchData searchData)
        {
            return BaseRepository.TableNoTracking;
        }

        public virtual IQueryable<T> GetList()
        {
            return GetList(new SearchData());
        }

        public virtual IQueryable<T> GetListByTrack()
        {
            return BaseRepository.Table;
        }

        public virtual (int Count, IQueryable<T> List) GetPagerList(SearchData searchData,
            DataOperation operation = null)
        {
            var query = BaseRepository.TableNoTracking;
            IOrderedQueryable<T> orderQuery;
            (int Count, IQueryable<T> Query) pagerQuery;

            if (operation != null)
            {
                if (operation.IsTrack)
                    query = BaseRepository.Table;
                orderQuery = query.OrderByDescending(r => r.CreateDate);

                if (!operation.UsePager) return (0, orderQuery);

                pagerQuery = orderQuery.Pager(searchData.ToPager());
                return (pagerQuery.Count, pagerQuery.Query);
            }

            orderQuery = query.OrderByDescending(r => r.CreateDate);
            pagerQuery = orderQuery.Pager(searchData.ToPager());
            return (pagerQuery.Count, pagerQuery.Query);
        }

        public virtual async Task<int> SaveAsync(T entity, bool alwaysUpdate = false)
        {
            if (entity.Id == Guid.Empty)
            {
                //新增
                await BaseRepository.InsertAsync(entity);
            }
            else
            {
                //通过强制附加，设置对象状态为修改
                if (alwaysUpdate) BaseRepository.Update(entity);
            }
            return await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            BaseRepository.Delete(entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(IEnumerable<T> entities)
        {
            BaseRepository.Delete(entities);
            return await _unitOfWork.SaveChangesAsync();
        }
        protected bool IsSpecialUser()
        {
            return CurrentUser != null && CurrentUser.SystemId > 2;
        }
    }
}