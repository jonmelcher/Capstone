// **************************************************************************
//  filename    :   GarageModelTest.cs
//  purpose     :   test out application - database functionality through the
//                  GarageRepository
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// **************************************************************************


using System;
using GarageModel;


namespace GarageModelTest
{
    class GarageModelTest
    {
        static void Main(string[] args)
        {
            GarageAssignment assignment = GarageRepository.GetGarageAssignment("39009D3D68");
            Console.WriteLine($"{assignment.ID}, {assignment.Stored}, {assignment.Tier}, {assignment.Cell}");
            GarageRepository.MoveVehicle("id", true);
            assignment = GarageRepository.GetGarageAssignment("39009D3D68");
            Console.WriteLine($"{assignment.ID}, {assignment.Stored}, {assignment.Tier}, {assignment.Cell}");
            Console.ReadKey();
        }
    }
}