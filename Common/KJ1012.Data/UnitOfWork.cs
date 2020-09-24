using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KJ1012.Core.Data;
using Microsoft.Data.SqlClient;

namespace KJ1012.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Kj1012Context _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(Kj1012Context dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }


        public DbContext GetDbContext() => _dbContext;

        public IRepository<T> Repository<T>() where T : class
        {
            return _serviceProvider.GetService<IRepository<T>>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public DataTable ExecDataTable(string strSql)
        {
            string strConn = _dbContext.Database.GetDbConnection().ConnectionString;
            SqlDataAdapter da = new SqlDataAdapter(strSql, strConn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataSet ExecDataSet(string strSql)
        {
            string strConn = _dbContext.Database.GetDbConnection().ConnectionString;
            SqlDataAdapter da = new SqlDataAdapter(strSql, strConn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public async Task ExecuteSqlCommandAsync(string sql)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }
        public void ExecuteSqlCommand(string sql)
        {
            _dbContext.Database.ExecuteSqlRaw(sql);
        }
    }
}