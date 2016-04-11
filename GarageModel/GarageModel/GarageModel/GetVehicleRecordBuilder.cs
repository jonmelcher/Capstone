using System.Data;

namespace GarageModel
{
    internal sealed class GetVehicleRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            Command.CommandText = "GetVehicleRecord";
            Command.Parameters.Add("@id", SqlDbType.Char);
        }
    }
}
