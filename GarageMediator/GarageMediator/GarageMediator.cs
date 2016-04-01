using System;
using GarageModel;
using SerialCommunications;
using System.Threading;


namespace GarageMediator
{
    public class GarageMediator
    {
        private volatile bool _isRunning;
        private RS232Server MicroCommunication { get; set; }
        private Parallax28140Server RFIDCommunication { get; set; }
        private Thread GarageWorker { get; set; }

        // TODO:    refactor GarageRepository in the singleton pattern
        //          use as a property

        // constructor
        public GarageMediator()
        {
            MicroCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.RS232) as RS232Server;
            RFIDCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;
        }



        private void GarageWorkerProcess()
        {
            while (_isRunning)
            {
                // retrieve current scanned RFID tag
                var id = RFIDCommunication.CurrentID;

                // if there is a scanned tag...
                if (id != string.Empty)
                {
                    // retrieve assignment from database
                    var assignment = GarageRepository.GetGarageAssignment(id);

                    // move vehicle in or out of garage depending on stored condition...
                    if (assignment.Stored)
                    {
                        // retrieve car from cell
                        throw new NotImplementedException();
                    }
                    else
                    {
                        // move car in empty cell to where it belongs in the garage
                        throw new NotImplementedException();
                    }

                    // update database after vehicle has been moved in or out of garage
                    GarageRepository.MoveVehicle(id, !assignment.Stored);

                    // clears scanner to start accepting a new scan
                    RFIDCommunication.ClearScanner();
                }

                // wait some amount of time between checks
                Thread.Sleep(250);
            }
        }

        public void StartCommunication()
        {
            if (_isRunning)
                return;

            MicroCommunication.StartServer();
            RFIDCommunication.StartServer();

            GarageWorker = new Thread(GarageWorkerProcess);
            GarageWorker.IsBackground = true;
            GarageWorker.Start();
        }

        public void StopCommunication()
        {
            if (!_isRunning)
                return;

            RFIDCommunication.StopServer();
            MicroCommunication.StopServer();
            _isRunning = false;
            GarageWorker.Join();
        }
    }
}