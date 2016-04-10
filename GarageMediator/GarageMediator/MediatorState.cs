// *************************************************************************
//  filename    :   MediatorState.cs
//  purpose     :   abstract base for the different states of GarageMediator
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-04-07
// *************************************************************************


using GarageModel;
using System;


namespace GarageMediator
{
    internal abstract class MediatorState
    {
        internal abstract void Change(GarageMediator context);

        internal virtual void ProcessVehicle(GarageAssignment assignment)
        {
            throw new InvalidOperationException();
        }

        internal virtual void Kill(GarageMediator context)
        {
            context.MicroCommunication.StopServer();
            context.RFIDCommunication.StopServer();
            context.State = new MediatorKilledState();
        }
    }
}