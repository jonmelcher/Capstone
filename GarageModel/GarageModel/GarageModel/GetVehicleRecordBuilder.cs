using System.Data;

namespace GarageModel
{
    internal sealed class GetVehicleRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            _command.CommandText = "GetVehicleRecord";
            _command.Parameters.Add("@id", SqlDbType.Char);
        }
    }
}
