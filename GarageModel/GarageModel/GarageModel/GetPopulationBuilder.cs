namespace GarageModel
{
    internal sealed class GetPopulationBuilder : GarageProcedureBuilder
    {
        public override void BuildCommand()
        {
            Command.CommandText = "GetPopulation";
        }
    }
}
