using Microsoft.EntityFrameworkCore;

namespace KJ1012.Core.Data
{
    public interface IViewRepository<T> where T : class
    {
        DbSet<T> View { get; }
    }
}
