using Microsoft.EntityFrameworkCore;
using KJ1012.Core.Data;
using KJ1012.Data.Entities;

namespace KJ1012.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public class ViewRepository<T> : IViewRepository<T> where T : BaseView
    {
        private readonly Kj1012Context _context;

        private DbSet<T> _view;


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public ViewRepository(Kj1012Context context)
        {
            _context = context;
        }

        #endregion
        public DbSet<T> View => _view ?? (_view = _context.Set<T>());
    }
}