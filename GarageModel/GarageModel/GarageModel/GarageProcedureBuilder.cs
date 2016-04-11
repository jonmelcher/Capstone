using System;
using System.Data;
using System.Data.SqlClient;


namespace GarageModel
{
    internal abstract class GarageProcedureBuilder
    {
        public SqlCommand Command { get; protected set; }

        protected GarageProcedureBuilder()
        {
            Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
        }

        public void SetConnection(SqlConnection connection)
        {
            Command.Connection = connection;
        }

        public void AssignParameters(params object[] args)
        {
            if ((args?.Length ?? 0) != Command.Parameters.Count)
                throw new ArgumentException();
            for (var i = 0; i < args.Length; ++i)
                Command.Parameters[i].Value = args[i];
        }

        public abstract void BuildCommand();
    }
}