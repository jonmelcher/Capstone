// ************************************************************************
//  filename    :   MediatorReadyState.cs
//  purpose     :   the initial state for GarageMediator
//                  the servers/repos have been initialized but not started
//                  only Change/Kill functionality should be available
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-04-07
// ************************************************************************


namespace GarageMediator
{
    internal sealed class MediatorReadyState : MediatorState
    {
        internal MediatorReadyState() { }

        // GarageMediator will request to start the servers, which will be granted
        // and its state moved into the listening state
        internal override void Change(GarageMediator context)
        {
            context.MicroCommunication.StartServer();
            context.RFIDCommunication.StartServer();
            context.State = new MediatorListeningState(context);
        }
    }
}