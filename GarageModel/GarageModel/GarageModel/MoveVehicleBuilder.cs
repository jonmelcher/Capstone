using System.Data;

namespace GarageModel
{
    internal sealed class MoveVehicleBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            Command.CommandText = "MoveVehicle";
            Command.Parameters.Add("@id", SqlDbType.Char);
            Command.Parameters.Add("@isGoingIn", SqlDbType.Bit);
        }
    }
}