

using System;
using System.Threading;
using System.Threading.Tasks;
using GarageModel;


namespace GarageMediator
{
    class MediatorListeningState : MediatorState
    {
        private volatile bool _isRunning;
        private string _currentID;
        private Task _worker;

        public static event Action<GarageAssignment, VehicleInformation> IDScanned;

        public MediatorListeningState()
        {
            _isRunning = true;
            _worker = Task.Run(() =>
            {
                while (_isRunning)
                {
                    ScanID(RFIDCommunication.CurrentID);
                    Thread.Sleep(0);
                }
            });
        }

        private void ScanID(string id)
        {
            if (id == string.Empty || _currentID != string.Empty)
                return;

            _currentID = id;
            IDScanned(DatabaseCommunication.GetGarageAssignment(id),
                   DatabaseCommunication.GetVehicleInformation(id));
        }

        public override void Change(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            context.State = new MediatorProcessingState();
        }

        public override void Kill(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            base.Kill(context);
        }
    }
}
