using System.Threading.Tasks;
using KJ1012.AppSetting.Models;
using KJ1012.Domain;

namespace KJ1012.AppSetting.IServices
{
    /// <summary>
    /// 数据库安装及初始化接口
    /// </summary>
    public interface ISqlInstallationService : IInstallationService
    {
        ResultEx ConnectTest(InstallSqlServerModel sqlServerModel);
        Task<ResultEx> InitDataBase(InstallSqlServerModel sqlServerModel);
    }
}