using System.Data;

namespace GarageModel
{
    internal sealed class GetVehicleInfoRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            Command.CommandText = "GetVehicleInfoRecord";
            Command.Parameters.Add("@id", SqlDbType.Char);
        }
    }
}