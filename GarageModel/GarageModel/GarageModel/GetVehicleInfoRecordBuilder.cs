using System.Data;

namespace GarageModel
{
    internal sealed class GetVehicleInfoRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            _command.CommandText = "GetVehicleInfoRecord";
            _command.Parameters.Add("@id", SqlDbType.Char);
        }
    }
}