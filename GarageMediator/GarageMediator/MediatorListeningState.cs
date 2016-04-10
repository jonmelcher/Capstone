// **********************************************************************************
//  filename    :   MediatorListeningState.cs
//  purpose     :   the second state for GarageMediator
//                  the servers/repos have been started but need to have listeners
//                  added.  only Change/Kill functionality should be available.
//                  an event is available to allow the GarageMediator to know when a
//                  valid RFID tag has been scanned so it can request to change state
//                  or to continue listening (by clearing the CurrentID)
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-04-07
// **********************************************************************************


using System;
using System.Threading;
using System.Threading.Tasks;


namespace GarageMediator
{
    internal sealed class MediatorListeningState : MediatorState
    {
        private volatile bool _isRunning;
        private Task _worker;

        // event to propogate upwards when a valid RFID tag has been scanned
        public static event Action<object, string> IDScanned;

        // current ID being scanned to guard against multiple scans of the same tag
        internal string CurrentID { get; set; }

        // constructor - sets up _worker task using context
        internal MediatorListeningState(GarageMediator context)
        {
            _isRunning = true;
            CurrentID = string.Empty;
            _worker = Task.Run(GetWorkerAction(context));
        }

        internal override void Change(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            context.State = new MediatorProcessingState(context);
        }

        internal override void Kill(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            base.Kill(context);
        }

        private Action GetWorkerAction(GarageMediator context)
        {
            return () =>
            {
                while (_isRunning)
                {
                    ScanID(context.RFIDCommunication.CurrentID);
                    Thread.Sleep(0);
                }
            };
        }

        private void ScanID(string id)
        {
            if (id == string.Empty || CurrentID != string.Empty)
                return;

            CurrentID = id;
            IDScanned(this, id);
        }
    }
}