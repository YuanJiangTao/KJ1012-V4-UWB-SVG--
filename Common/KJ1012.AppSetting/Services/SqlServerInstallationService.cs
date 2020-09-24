using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KJ1012.AppSetting.IServices;
using KJ1012.AppSetting.Models;
using KJ1012.Core.Data;
using KJ1012.Core.Data.Sql_Server;
using KJ1012.Core.Encrypt;
using KJ1012.Data;
using KJ1012.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace KJ1012.AppSetting.Services
{
    public class SqlServerInstallationService : ISqlInstallationService
    {
        private readonly IAppSettingService _appSettingService;
        private const string SqlServerScriptPath = "App_Data\\Install\\SqlServer";
        private string _dataBaseName = "";

        public SqlServerInstallationService(IAppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }
        public ResultEx ConnectTest(InstallSqlServerModel sqlServerModel)
        {
            var result = CheckSqlServerParameter(sqlServerModel);
            if (result.Flag)
            {
                string connectionString;
                if (sqlServerModel.ConnectionString != null)
                    sqlServerModel.ConnectionString = sqlServerModel.ConnectionString.Trim();
                if (sqlServerModel.ConnectionType == ConnectionType.String)
                    connectionString = sqlServerModel.ConnectionString;
                else
                    connectionString = CreateConnectionString(
                        sqlServerModel.AuthenticationType == AuthenticationType.Windows,
                        sqlServerModel.ServerName, sqlServerModel.DatabaseName, sqlServerModel.Username,
                        sqlServerModel.Password);
                try
                {
                    //ͨ���������ݿⷽʽȷ��
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        conn.Close();
                    }
                    return ResultEx.Init(true);
                }
                catch (Exception ex)
                {
                    return ResultEx.Init(false, ex.Message);
                }
            }

            return result;
        }

        public async Task<ResultEx> InitDataBase(InstallSqlServerModel sqlServerModel)
        {
            var result = CheckSqlServerParameter(sqlServerModel);
            if (result.Flag)
            {
                _dataBaseName = result.Data.ToString();
                string connectionString;
                if (!string.IsNullOrEmpty(sqlServerModel.ConnectionString))
                    sqlServerModel.ConnectionString = sqlServerModel.ConnectionString.Trim();
                if (sqlServerModel.ConnectionType == ConnectionType.String)
                {
                    connectionString = sqlServerModel.ConnectionString;
                }
                else
                {
                    connectionString = CreateConnectionString(
                        sqlServerModel.AuthenticationType == AuthenticationType.Windows,
                        sqlServerModel.ServerName, sqlServerModel.DatabaseName, sqlServerModel.Username,
                        sqlServerModel.Password);
                }

                //�����µ�DBContext����ֻ�����ݿⴴ������
                var builder = new DbContextOptionsBuilder();
                builder.UseSqlServer(connectionString);
                var dbContext = new Kj1012Context(builder.Options);
                var dataBase = dbContext.Database;
                var databaseCreator = dataBase.GetService<IRelationalDatabaseCreator>();
                sqlServerModel.ConnectionString = connectionString;
                var isCreateDatabase = false;
                if (sqlServerModel.AlwaysCreate)
                {
                    if (databaseCreator.Exists())
                    {
                        await databaseCreator.DeleteAsync();
                    }
                    await CreateDataBase(databaseCreator, dataBase);
                    isCreateDatabase = true;
                }
                else if (sqlServerModel.NotExistCreate)
                {
                    if (!databaseCreator.Exists())
                    {
                        await CreateDataBase(databaseCreator, dataBase);
                        isCreateDatabase = true;
                    }
                }
                else
                {
                    if (!databaseCreator.Exists())
                    {
                        return ResultEx.Init(false, "���ݿ����Ʋ����ڣ���˶�");
                    }
                }
                var version = GetSqlServerVersion(sqlServerModel.ConnectionString);
                if (version < 10)
                {
                    return ResultEx.Init(false, "�������ݱ�ʧ�ܣ�Sql Server ���ݿ�汾������ҪSql Server 2008������");
                }
                if (version > 0)
                {
                    //SQL Server2016 ������ִ��2016�ű�
                    if (version >= 13 && isCreateDatabase)
                    {
                        //ִ��2016�ű�
                        ExecuteSqlScriptFromFile(dataBase, $"{SqlServerScriptPath}\\SqlServer_2016.sql");
                    }
                    //�������ݿ�������Ϣ
                    _appSettingService.SaveDataSetting(new DataSettings
                    {
                        DataProvider = DataProviderType.SqlServer,
                        Version = version,
                        ConnectionString = AesHelper.AesEncrypt(sqlServerModel.ConnectionString, ConstDefine.DataSettingAesKey)
                    });
                }
                else
                {
                    return ResultEx.Init(false, "�������ݱ�ʧ�ܣ�δ֪SQL SERVER���ݿ�汾");
                }
            }

            return result;
        }

        private async Task CreateDataBase(IRelationalDatabaseCreator databaseCreator, DatabaseFacade dataBase)
        {
            //�������ݿ�
            await databaseCreator.CreateAsync();
            var sqlScript = dataBase.GenerateCreateScript();
            //�������ݱ�
            var sqlCommands = GetCommandsFromScript(sqlScript);
            foreach (var command in sqlCommands)
                await dataBase.ExecuteSqlRawAsync(command);
            //������ͼ
            ExecuteSqlScriptFromFile(dataBase, $"{SqlServerScriptPath}\\SqlServer_Views.sql");
            //�����洢����
            ExecuteSqlScriptFromFile(dataBase, $"{SqlServerScriptPath}\\SqlServer_StoredProcedures.sql");
            //ִ�д洢����
            ExecuteProcedure(dataBase, "Update_DataBase @dataBase = @p0", _dataBaseName);
            //�����Զ���ű�
            ExecuteSqlScriptFromFile(dataBase, $"{SqlServerScriptPath}\\SqlServer_Custom.sql");
            //��ʼ�����ݽű�
            ExecuteSqlScriptFromFile(dataBase, $"{SqlServerScriptPath}\\SqlServer_Data.sql");
        }
        private string CreateConnectionString(bool trustedConnection, string serverName, string databaseName, string userName,
            string password, int timeout = 0)
        {
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = trustedConnection,
                DataSource = serverName,
                InitialCatalog = databaseName,
                MaxPoolSize = 200
            };
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = true;
            if (timeout > 0)
            {
                builder.ConnectTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        private ResultEx CheckSqlServerParameter(InstallSqlServerModel model)
        {
            if (model.ConnectionString != null)
                model.ConnectionString = model.ConnectionString.Trim();
            if (model.ConnectionType == ConnectionType.String)
            {
                //�Զ��������ַ���
                if (string.IsNullOrEmpty(model.ConnectionString))
                    return new ResultEx("���ݿ��ַ�������Ϊ��");
                try
                {
                    //���Թ������Ӷ���
                    var builder = new SqlConnectionStringBuilder(model.ConnectionString);
                    return new ResultEx(true, "", builder.InitialCatalog);
                }
                catch (Exception ex)
                {
                    return new ResultEx($"���ݿ��ַ�����ʽ����ȷ��{ex.Message}");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.ServerName))
                    return new ResultEx("���ݿ�ʵ����ַ����Ϊ��");
                if (string.IsNullOrEmpty(model.DatabaseName))
                    return new ResultEx("���ݿ����Ʋ���Ϊ��");

                //��Ȩ�˺�����
                if (model.AuthenticationType == AuthenticationType.SqlServer)
                {
                    if (string.IsNullOrEmpty(model.Username))
                        return new ResultEx("SQL Server�û�������Ϊ��");
                    if (string.IsNullOrEmpty(model.Password))
                        return new ResultEx("SQL Server���벻��Ϊ��");
                }
                return new ResultEx(true, "", model.DatabaseName);
            }
        }

        private int GetSqlServerVersion(string connectionString)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string serverVersion = sqlConnection.ServerVersion;
                    string[] serverVersionDetails = serverVersion.Split('.');

                    return int.Parse(serverVersionDetails[0]);
                }
            }
            catch
            {
                return 0;
            }
        }
        private static IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty);
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            for (var i = 0; i < batches.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) || batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                    continue;

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                        count = int.Parse(match.Value);
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                        builder.AppendLine();
                }

                commands.Add(builder.ToString());
            }

            return commands;
        }
        private void ExecuteSqlScriptFromFile(DatabaseFacade database, string filePath)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            if (!File.Exists(filePath))
                return;

            var sql = File.ReadAllText(filePath);
            var sqlCommands = GetCommandsFromScript(sql);
            foreach (var command in sqlCommands)
                database.ExecuteSqlRaw(command);
        }
        private void ExecuteProcedure(DatabaseFacade database, string procedureName, params object[] parameters)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            database.ExecuteSqlRaw($"EXEC {procedureName}", parameters);
        }
    }
}