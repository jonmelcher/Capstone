namespace GarageModel
{
    internal sealed class GetPopulationBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            _command.CommandText = "GetPopulation";
        }
    }
}
