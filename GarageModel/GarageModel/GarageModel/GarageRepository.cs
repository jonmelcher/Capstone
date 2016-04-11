// **********************************************************************************
//  filename    :   GarageRepository.cs
//  purpose     :   encapsulates SELECT and UPDATE access to the Garage Database
//                  through stored procedures.  all database access should go through
//                  this class.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// **********************************************************************************


using System;
using System.Data;
using System.Data.SqlClient;


namespace GarageModel
{
    public sealed class GarageRepository
    {
        private static GarageRepository _instance;
        private GarageRepository() { }

        public static GarageRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GarageRepository();
                return _instance;
            }
        }

        public int GetGaragePopulation()
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = GetSqlCommand(connection, "GetPopulation"))
            {
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    if (!reader.HasRows)
                        return -1;

                    reader.Read();
                    return (int)reader[VehicleStatisticHeaders.Population.ToString()];
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            }

            return -1;
        }


        public VehicleInformation GetVehicleInformation(string id)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = GetSqlCommand(connection, "GetVehicleInfoRecord"))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    return new VehicleInformation((string)reader[VehicleInfoHeaders.VehicleID.ToString()],
                                                        (int)reader[VehicleInfoHeaders.Mileage.ToString()],
                                                 (DateTime)reader[VehicleInfoHeaders.ModelYear.ToString()],
                                                        (string)reader[VehicleInfoHeaders.Make.ToString()],
                                                       (string)reader[VehicleInfoHeaders.Model.ToString()],
                                                      (string)reader[VehicleInfoHeaders.Colour.ToString()],
                                                       (string)reader[VehicleInfoHeaders.Notes.ToString()]);
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            }

            return null;
        }

        private SqlCommand GetSqlCommand(SqlConnection connection, string procedureName)
        {
            var command = new SqlCommand();
            command.CommandText = procedureName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }


        public bool UpdateVehicleInformation(string id, int mileage, string colour, string notes)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = GetSqlCommand(connection, "UpdateVehicleInfoRecord"))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@mileage", mileage));
                command.Parameters.Add(new SqlParameter("@colour", colour));
                command.Parameters.Add(new SqlParameter("@notes", notes));

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

        // retrieve the complete record of a Vehicle in the Vehicles Table
        // based on the given id (primary key)
        public GarageAssignment GetGarageAssignment(string id)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            using (var command = GetSqlCommand(connection, "GetVehicleRecord"))
            {
                command.Parameters.Add(new SqlParameter("@id", id));

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (var command = GetSqlCommand(connection, "MoveVehicle"))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@isGoingIn", isGoingIn));

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
        private enum VehicleInfoHeaders { VehicleID, Mileage, ModelYear, Make, Model, Colour, Notes }
        private enum VehicleStatisticHeaders { Population }
    }
}