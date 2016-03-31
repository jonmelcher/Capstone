// *********************************************************************************
//  filename    :   GarageAssignment.cs
//  purpose     :   immutable container for data returned from Vehicles Table in the
//                  Garage Database.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// *********************************************************************************  


namespace GarageModel
{
    public class GarageAssignment
    {
        private readonly string _id;
        private readonly int _tier;
        private readonly int _cell;
        private readonly bool _stored;

        public string ID => _id;
        public bool Stored => _stored;
        public int Tier => _tier;
        public int Cell => _cell;

        public GarageAssignment(string id, bool stored, int tier, int cell)
        {
            _id = id;
            _stored = stored;
            _tier = tier;
            _cell = cell;
        }

        public GarageAssignment(GarageAssignment assignment, bool stored) :
            this(assignment.ID, stored, assignment.Tier, assignment.Cell)
        { }
    }
}