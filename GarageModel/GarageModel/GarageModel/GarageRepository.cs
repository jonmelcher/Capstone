// **********************************************************************************
//  filename    :   GarageRepository.cs
//  purpose     :   encapsulates SELECT and UPDATE access to the Garage Database
//                  through stored procedures.  all database access should go through
//                  this class.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// **********************************************************************************


using System;
using System.Collections.Generic;
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
            {
                var director = new GarageProcedureDirector(connection, new GetPopulationBuilder());
                using (var command = director.Construct())
                {
                    var results = GetQueryResults(command);
                    return results.Count > 0
                        ? (int)results[0][0]
                        : -1;
                }
            }
        }

        public VehicleInformation GetVehicleInformation(string id)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            {
                var director = new GarageProcedureDirector(connection, new GetVehicleInfoRecordBuilder());
                using (var command = director.Construct(id))
                {
                    var results = GetQueryResults(command);
                    return results.Count > 0
                        ? new VehicleInformation(results[0])
                        : null;
                }
            }
        }

        public bool UpdateVehicleInformation(string id, int mileage, string colour, string notes)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            {
                var director = new GarageProcedureDirector(connection, new UpdateVehicleInfoRecordBuilder());
                using (var command = director.Construct(id, mileage, colour, notes))
                    return GetNonQueryResults(command);
            }
        }

        // retrieve the complete record of a Vehicle in the Vehicles Table
        // based on the given id (primary key)
        public GarageAssignment GetGarageAssignment(string id)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            {
                var director = new GarageProcedureDirector(connection, new GetVehicleRecordBuilder());
                using (var command = director.Construct(id))
                {
                    var results = GetQueryResults(command);
                    return results.Count > 0 ?
                        new GarageAssignment(results[0])
                        : null;
                }
            }
        }

        // update the Vehicle record in the Vehicles Table associated with the given id to
        // reflect whether the Vehicle is inside the garage or not.
        public bool MoveVehicle(string id, bool isGoingIn)
        {
            using (var connection = new SqlConnection(DataSource.ConnectionString))
            {
                var director = new GarageProcedureDirector(connection, new MoveVehicleBuilder());
                using (var command = director.Construct(id, isGoingIn))
                    return GetNonQueryResults(command);
            }
        }

        private List<object[]> GetQueryResults(SqlCommand command)
        {
            var results = new List<object[]>();
            try
            {
                command.Connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    var result = new object[reader.FieldCount];
                    reader.GetValues(result);
                    results.Add(result);
                }
            }
            catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            return results;
        }

        private bool GetNonQueryResults(SqlCommand command)
        {
            try
            {
                command.Connection.Open();
                return command.ExecuteNonQuery() >= 0;
            }
            catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            return false;
        }
    }
}