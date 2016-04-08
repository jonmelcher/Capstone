

using System;
using System.Threading;
using System.Threading.Tasks;


namespace GarageMediator
{
    class MediatorListeningState : MediatorState
    {
        private volatile bool _isRunning;
        private string _currentID;
        private Task _worker;

        public static event Action<string> IDScanned;

        public MediatorListeningState(GarageMediator context)
        {
            _isRunning = true;
            _worker = Task.Run(() =>
            {
                while (_isRunning)
                {
                    ScanID(context.RFIDCommunication.CurrentID);
                    Thread.Sleep(0);
                }
            });
        }

        private void ScanID(string id)
        {
            if (id == string.Empty || _currentID != string.Empty)
                return;

            _currentID = id;
            IDScanned(id);
        }

        public override void Change(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            context.State = new MediatorProcessingState(context);
        }

        public override void Kill(GarageMediator context)
        {
            _isRunning = false;
            _worker.Wait();
            base.Kill(context);
        }
    }
}