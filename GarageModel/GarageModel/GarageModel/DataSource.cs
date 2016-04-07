// *********************************************************************
//  filename    :   DataSource.cs
//  purpose     :   provide a common connection string for accessing the
//                  Garage database.  employs the singleton pattern
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// *********************************************************************


using System.Data.SqlClient;


namespace GarageModel
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