using System;
using System.Data.SqlClient;


namespace Capstone
{
    public static class GarageRepository
    {
        private static readonly string connectionString = "user id=*****;" +
                                                          "password=*****;" +
                                                          "server=bender.net.nait.ca,24680;" +
                                                          "Trusted_Connection=yes;" +
                                                          "database=*****;" +
                                                          "Integrated Security = false;" +
                                                          "connection timeout=30";

        private static readonly string vehiclesTableName = "Vehicles";
        private static readonly string[] vehiclesTableHeaders = new string[]
        {
            "VehicleID", "Stored", "Tier", "Cell"
        };

        public static void UpdateAssignment(GarageAssignment assignment)
        {
            string sqlQuery = $"UPDATE {vehiclesTableName} SET {vehiclesTableHeaders[1]} = @stored"
                            + $" WHERE {vehiclesTableHeaders[0]} = @id";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@stored", assignment.Stored);
                    command.Parameters.AddWithValue("@id", assignment.ID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public static GarageAssignment GetAssignment(string id)
        {
            string sqlQuery = $"SELECT {string.Join(", ", vehiclesTableHeaders)} FROM {vehiclesTableName}"
                            + $" WHERE VehicleID = '{id}'";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(sqlQuery, connection);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    return new GarageAssignment((string)reader[vehiclesTableHeaders[0]],
                                                (bool)reader[vehiclesTableHeaders[1]],
                                                (int)reader[vehiclesTableHeaders[2]],
                                                (int)reader[vehiclesTableHeaders[3]]);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}
