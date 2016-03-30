using System.Data.SqlClient;

namespace Capstone
{
    internal static class DataSource
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = GetConnectionString();
                return _connectionString;
            }
        }

        private static string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.UserID = "*****";
            builder.Password = "*****";
            builder.InitialCatalog = "*****";
            builder.DataSource = "*****";
            builder.ConnectTimeout = 30;
            return builder.ConnectionString;
        }
    }
}
