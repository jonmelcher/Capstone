using System.Data.SqlClient;


namespace GarageModel
{
    internal sealed class GarageProcedureDirector
    {
        private SqlConnection _connection;
        private GarageProcedureBuilder _builder;

        public GarageProcedureDirector(SqlConnection connection, GarageProcedureBuilder builder)
        {
            _connection = connection;
            _builder = builder;
        }

        public SqlCommand Construct(params object[] args)
        {
            _builder.SetConnection(_connection);
            _builder.BuildCommand();
            _builder.AssignParameters(args);
            return _builder.Command;
        }
    }
}