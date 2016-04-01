// **********************************************************************************
//  filename    :   GarageRepository.cs
//  purpose     :   encapsulates SELECT and UPDATE access to the Garage Database
//                  through stored procedures.  all database access should go through
//                  this class.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// **********************************************************************************


using System;
using System.Data.SqlClient;


namespace GarageModel
{
    public class GarageRepository
    {
        private GarageRepository _instance;
        private GarageRepository() { }

        public GarageRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GarageRepository();
                return _instance;
            }
        }

        // retrieve the complete record of a Vehicle in the Vehicles Table
        // based on the given id (primary key)
        public GarageAssignment GetGarageAssignment(string id)
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
                                                        (byte)reader[VehiclesHeaders.Cell.ToString()]);
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            }

            return null;
        }

        // update the Vehicle record in the Vehicles Table associated with the given id to
        // reflect whether the Vehicle is inside the garage or not.
        public bool MoveVehicle(string id, bool isGoingIn)
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
                    return true;
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }

                return false;
            }
        }

        // headers for Vehicles Table in Garage Database
        private enum VehiclesHeaders { VehicleID, Stored, Cell }
    }
}