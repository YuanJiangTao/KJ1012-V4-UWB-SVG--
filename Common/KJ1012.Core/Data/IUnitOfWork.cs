using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KJ1012.Core.Data
{
    public interface IUnitOfWork
    {
        DbContext GetDbContext();
        IRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync();
        int SaveChanges();
        DataTable ExecDataTable(string strSql);
        DataSet ExecDataSet(string strSql);
        Task ExecuteSqlCommandAsync(string sql);
        void ExecuteSqlCommand(string sql);
    }
}
