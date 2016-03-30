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
            builder.UserID = "jmelcher1";
            builder.Password = "00Hobokin00";
            builder.InitialCatalog = "jmelcher1";
            builder.DataSource = "bender.net.nait.ca,24680";
            builder.ConnectTimeout = 30;
            return builder.ConnectionString;
        }
    }
}
