// *********************************************************************************
//  filename    :   GarageAssignment.cs
//  purpose     :   immutable container for data returned from Vehicles Table in the
//                  Garage Database.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-29
// *********************************************************************************  


using System;
using System.Linq;

namespace GarageModel
{
    public sealed class GarageAssignment
    {
        private readonly string _id;
        private readonly byte _cell;
        private readonly bool _stored;

        public string ID => _id;
        public bool Stored => _stored;
        public byte Cell => _cell;

        public GarageAssignment(object[] sqlResult)
        {
            if (sqlResult == null || sqlResult.Length != GetType().GetProperties().Count())
                throw new ArgumentException();
            _id = (string)sqlResult[0];
            _stored = (bool)sqlResult[1];
            _cell = (byte)sqlResult[2];
        }

        public GarageAssignment(string id, bool stored, byte cell)
        {
            _id = id;
            _stored = stored;
            _cell = cell;
        }

        public GarageAssignment(GarageAssignment assignment, bool stored) :
            this(assignment.ID, stored, assignment.Cell) { }

        public override string ToString()
        {
            return $"ID: {ID}\nStored: {Stored}\nCell: {Cell}";
        }
    }
}