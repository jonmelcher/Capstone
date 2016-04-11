

using System;
using System.Linq;

namespace GarageModel
{
    public sealed class VehicleInformation
    {
        public string ID { get; private set; }
        public int Mileage { get; set; }
        public DateTime ModelYear { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public string Colour { get; set; }
        public string Notes { get; set; }

        public VehicleInformation(object[] sqlResult)
        {
            if (sqlResult == null || sqlResult.Length != GetType().GetProperties().Count())
                throw new ArgumentException();

            ID = (string)sqlResult[0];
            Mileage = (int)sqlResult[1];
            ModelYear = (DateTime)sqlResult[2];
            Make = (string)sqlResult[3];
            Model = (string)sqlResult[4];
            Colour = (string)sqlResult[5];
            Notes = (string)sqlResult[6];
        }

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