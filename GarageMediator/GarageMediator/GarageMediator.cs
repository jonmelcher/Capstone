// ***********************************************************************************
//  filename    :   GarageMediator.cs
//  purpose     :   provide a central point for inter-communication between the serial
//                  servers and the database while running the Garage
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-04-07
// ***********************************************************************************


using System;
using GarageModel;
using SerialCommunications;


namespace GarageMediator
{
    public sealed class GarageMediator
    {
        internal GarageRepository DatabaseCommunication { get; private set; }
        internal RS232Server MicroCommunication { get; private set; }
        internal Parallax28140Server RFIDCommunication { get; private set; }
        internal MediatorState State { get; set; }

        // constructor - sets up the initial state and servers/database repo without starting
        public GarageMediator()
        {
            DatabaseCommunication = GarageRepository.Instance;
            MicroCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.RS232) as RS232Server;
            RFIDCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;
            MediatorListeningState.IDScanned += (sender, id) => IDScanned(sender, DatabaseCommunication.GetGarageAssignment(id),
                                                                               DatabaseCommunication.GetVehicleInformation(id));
            State = new MediatorReadyState();
        }

        public string GetStatus()
        {
            if (State is MediatorKilledState)
                return "Disconnected.";
            if (State is MediatorReadyState)
                return "Ready to Start.";
            if (State is MediatorListeningState)
                return "Listening for RFID Tag.";
            return "Waiting for User Input.";
        }

        // event to propogate upwards when an RFID tag is scanned
        public event Action<object, GarageAssignment, VehicleInformation> IDScanned;

        // event to propogate upwards when the mechanical part of the garage has finished
        // processing a vehicle and is waiting for another RFID tag
        public event Action VehicleProcessed
        {
            add { MediatorProcessingState.VehicleProcessed += value; }
            remove { MediatorProcessingState.VehicleProcessed -= value; }
        }

        public event Action VehicleInstructionsStarted
        {
            add { MediatorProcessingState.VehicleInstructionsStarted += value; }
            remove { MediatorProcessingState.VehicleInstructionsStarted -= value; }
        }

        public event Action VehicleProcessingStarted
        {
            add { MediatorProcessingState.VehicleProcessingStarted += value; }
            remove { MediatorProcessingState.VehicleProcessingStarted += value; }
        }

        // request to change state is propogated downwards into the MediatorState
        public void Request()
        {
            State.Change(this);
        }

        public void RequestClearID()
        {
            if (State is MediatorListeningState)
                (State as MediatorListeningState).CurrentID = string.Empty;
        }

        public void RequestProcessVehicle(GarageAssignment assignment)
        {
            if (State is MediatorProcessingState)
                (State as MediatorProcessingState).ProcessVehicle(assignment);
        }

        public void Kill()
        {
            State.Kill(this);
        }
    }
}