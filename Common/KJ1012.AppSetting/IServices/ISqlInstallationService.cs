using System.Threading.Tasks;
using KJ1012.AppSetting.Models;
using KJ1012.Domain;

namespace KJ1012.AppSetting.IServices
{
    /// <summary>
    /// ���ݿⰲװ����ʼ���ӿ�
    /// </summary>
    public interface ISqlInstallationService : IInstallationService
    {
        ResultEx ConnectTest(InstallSqlServerModel sqlServerModel);
        Task<ResultEx> InitDataBase(InstallSqlServerModel sqlServerModel);
    }
}