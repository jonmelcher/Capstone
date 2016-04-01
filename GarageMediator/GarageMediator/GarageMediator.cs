using System;
using GarageModel;
using SerialCommunications;
using System.Threading;


namespace GarageMediator
{
    public class GarageMediator
    {
        private const byte START_INSTRUCTION = 0xF0;
        private const byte CONTINUE_INSTRUCTION = 0xF1;
        private const byte STOP_INSTRUCTION = 0xF2;
        private const byte INSTRUCTIONS_COMPLETED = 0xF3;
        private const byte COMMUNICATION_DELAY_MS = 250;
        private const byte OUTGOING = 0x20;
        private const byte INCOMING = 0x21;

        private volatile bool _isRunning;
        private RS232Server MicroCommunication { get; set; }
        private Parallax28140Server RFIDCommunication { get; set; }
        private GarageRepository DatabaseCommunication { get; set; }
        private Thread GarageWorker { get; set; }

        // constructor
        public GarageMediator()
        {
            MicroCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.RS232) as RS232Server;
            RFIDCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;
            DatabaseCommunication = GarageRepository.Instance;
        }

        private void TransmitInstructions(byte garageCell, bool isGoingIntoCell)
        {
            MicroCommunication.Write(START_INSTRUCTION);
            WaitForByte(CONTINUE_INSTRUCTION);
            MicroCommunication.Write(garageCell);
            WaitForByte(CONTINUE_INSTRUCTION);
            MicroCommunication.Write(isGoingIntoCell ? INCOMING : OUTGOING);
            WaitForByte(CONTINUE_INSTRUCTION);
            MicroCommunication.Write(STOP_INSTRUCTION);
        }

        private void WaitForByte(byte b)
        {
            while (MicroCommunication.Read() != b)
                Thread.Sleep(COMMUNICATION_DELAY_MS);
        }

        private void ProcessScan(string id)
        {
            if (id == string.Empty)
                return;

            // retrieve assignment from database
            var assignment = DatabaseCommunication.GetGarageAssignment(id);

            // send instructions to the micro on which cell to retrieve/extract from,
            // and wait for completion of instructions
            TransmitInstructions(assignment.Cell, !assignment.Stored);
            WaitForByte(INSTRUCTIONS_COMPLETED);

            // update database after vehicle has been moved in or out of garage
            DatabaseCommunication.MoveVehicle(id, !assignment.Stored);

            // clears scanner to start accepting a new scan
            RFIDCommunication.ClearScanner();
        }

        private void GarageWorkerProcess()
        {
            while (_isRunning)
            {
                // process currently scanned RFID tag
                ProcessScan(RFIDCommunication.CurrentID);
                Thread.Sleep(COMMUNICATION_DELAY_MS);
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