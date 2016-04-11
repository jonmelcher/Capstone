using System.Data;

namespace GarageModel
{
    internal sealed class UpdateVehicleInfoRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            Command.CommandText = "UpdateVehicleInfoRecord";
            Command.Parameters.Add("@id", SqlDbType.Char);
            Command.Parameters.Add("@mileage", SqlDbType.Int);
            Command.Parameters.Add("@colour", SqlDbType.VarChar);
            Command.Parameters.Add("@notes", SqlDbType.VarChar);
        }
    }
}