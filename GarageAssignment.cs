namespace Capstone
{
    public class GarageAssignment
    {
      private readonly string _id;
      private readonly int _tier;
      private readonly int _cell;
      private readonly bool _stored;
    
      public GarageAssignment(string id, bool stored, int tier, int cell)
      {
          _id = id;
          _stored = stored;
          _tier = tier;
          _cell = cell;
      }
    
      public GarageAssignment(GarageAssignment assignment, bool stored) :
          this(assignment.ID, stored, assignment.Tier, assignment.Cell) { }
    
      public string ID => _id;
      public bool Stored => _stored;
      public int Tier => _tier;
      public int Cell => _cell;
    }
}
