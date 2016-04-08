// *******************************************************************************************
//  filename    :   MediatorKilledState.cs
//  purpose     :   an end state for GarageMediator
//                  the servers/repos have been killed.  only Change/Kill functionality
//                  is available, where Kill does nothing.  Change restarts the GarageMediator
//                  in the ready state.
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-04-07
// *******************************************************************************************


namespace GarageMediator
{
    public class MediatorKilledState : MediatorState
    {
        public override void Change(GarageMediator context)
        {
            context.State = new MediatorReadyState();
        }
    }
}