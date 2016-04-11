using System.Data;

namespace GarageModel
{
    internal sealed class UpdateVehicleInfoRecordBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            _command.CommandText = "UpdateVehicleInfoRecord";
            _command.Parameters.Add("@id", SqlDbType.Char);
            _command.Parameters.Add("@mileage", SqlDbType.Int);
            _command.Parameters.Add("@colour", SqlDbType.VarChar);
            _command.Parameters.Add("@notes", SqlDbType.VarChar);
        }
    }
}