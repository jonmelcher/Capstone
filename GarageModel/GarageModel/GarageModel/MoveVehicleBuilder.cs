using System.Data;

namespace GarageModel
{
    internal sealed class MoveVehicleBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            _command.CommandText = "MoveVehicle";
            _command.Parameters.Add("@id", SqlDbType.Char);
            _command.Parameters.Add("@isGoingIn", SqlDbType.Bit);
        }
    }
}