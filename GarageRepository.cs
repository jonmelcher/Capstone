using System;
using System.Data.SqlClient;


namespace Capstone
{
    public static class GarageRepository
    {
        private enum VehiclesHeaders { VehicleID, Stored, Tier, Cell }

        public static GarageAssignment GetGarageAssignment(string id)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = new SqlCommand())
            {
                command.CommandText = "GetVehicleRecord";
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    return new GarageAssignment((string)reader[VehiclesHeaders.VehicleID.ToString()],
                                                        (bool)reader[VehiclesHeaders.Stored.ToString()],
                                                        (int)reader[VehiclesHeaders.Tier.ToString()],
                                                        (int)reader[VehiclesHeaders.Cell.ToString()]);
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            }

            return null;
        }

        public static void MoveVehicle(string id, bool isGoingIn)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = new SqlCommand())
            {
                command.CommandText = "MoveVehicle";
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@inOrOut", isGoingIn));

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            }
        }
    }
}
