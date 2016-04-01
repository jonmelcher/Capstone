﻿using System;
using GarageModel;
using SerialCommunications;
using System.Threading;


namespace GarageMediator
{
    public class GarageMediator
    {
        // TODO:    determine values for this when communicating with microprocessor
        private const byte START_INSTRUCTIONS = 0x00;
        private const byte CONTINUE_INSTRUCTIONS = 0x00;
        private const byte STOP_INSTRUCTIONS = 0x00;
        private const byte INSTRUCTIONS_COMPLETED = 0x00;
        private const byte COMMUNICATION_DELAY_MS = 250;

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

        private void TransmitInstructions(byte garageCell, bool isGoingIntoCell)
        {
            MicroCommunication.Write(START_INSTRUCTIONS);
            WaitForByte(CONTINUE_INSTRUCTIONS);
            MicroCommunication.Write(garageCell);
            WaitForByte(CONTINUE_INSTRUCTIONS);
            MicroCommunication.Write(Convert.ToByte(isGoingIntoCell));
            WaitForByte(CONTINUE_INSTRUCTIONS);
            MicroCommunication.Write(STOP_INSTRUCTIONS);
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
            var assignment = GarageRepository.GetGarageAssignment(id);

            // send instructions to the micro on which cell to retrieve/extract from,
            // and wait for completion of instructions
            TransmitInstructions(assignment.Cell, !assignment.Stored);
            WaitForByte(INSTRUCTIONS_COMPLETED);

            // update database after vehicle has been moved in or out of garage
            GarageRepository.MoveVehicle(id, !assignment.Stored);

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