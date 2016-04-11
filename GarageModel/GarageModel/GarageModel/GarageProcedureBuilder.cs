using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GarageModel
{
    internal abstract class GarageProcedureBuilder
    {
        protected SqlCommand _command;

        protected GarageProcedureBuilder()
        {
            _command = new SqlCommand();
            _command.CommandType = CommandType.StoredProcedure;
        }

        public void SetConnection(SqlConnection connection)
        {
            _command.Connection = connection;
        }

        public SqlCommand GetCommand()
        {
            return _command;
        }

        public void AssignParameters(params object[] args)
        {
            if ((args?.Length ?? 0) != _command.Parameters.Count)
                throw new ArgumentException();
            for (var i = 0; i < args.Length; ++i)
                _command.Parameters[0].Value = args[i];
        }

        public abstract void BuildCommand();
    }
}
