using KJ1012.Core.Data.Sql_Server;

namespace KJ1012.AppSetting.Models
{
    public class InstallSqlServerModel
    {
        public string ConnectionString { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public bool NotExistCreate { get; set; }
        public bool AlwaysCreate { get; set; }

    }
}
