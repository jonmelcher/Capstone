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
            var repo = GarageRepository.Instance;

            GarageAssignment assignment = repo.GetGarageAssignment("39009D3D68");
            Console.WriteLine($"{assignment.ID}, {assignment.Stored}, {assignment.Cell}");
            repo.MoveVehicle("39009D3D68", true);
            assignment = repo.GetGarageAssignment("39009D3D68");
            Console.WriteLine($"{assignment.ID}, {assignment.Stored}, {assignment.Cell}");
            Console.WriteLine(repo.GetGaragePopulation());
            Console.ReadKey();
        }
    }
}