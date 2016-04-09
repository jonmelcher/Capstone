

using System;


namespace GarageModel
{
    public class VehicleInformation
    {
        public string ID { get; private set; }
        public int Mileage { get; set; }
        public DateTime ModelYear { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public string Colour { get; set; }
        public string Notes { get; set; }

        public VehicleInformation(string id, int mileage, DateTime modelYear, string make, string model, string colour, string notes)
        {
            ID = id;
            Mileage = mileage;
            ModelYear = modelYear;
            Make = make;
            Model = model;
            Colour = colour;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"Mileage: {Mileage}\nModelYear: {ModelYear}\nMake: {Make}\nModel: {Model}\nColour: {Colour}\n\n{Notes}";
        }
    }
}