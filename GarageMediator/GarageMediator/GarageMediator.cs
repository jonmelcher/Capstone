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
    public class GarageMediator
    {
        public GarageRepository DatabaseCommunication { get; private set; }
        public RS232Server MicroCommunication { get; private set; }
        public Parallax28140Server RFIDCommunication { get; private set; }
        public MediatorState State { get; set; }

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

        // event to propogate upwards when an RFID tag is scanned
        public event Action<object, GarageAssignment, VehicleInformation> IDScanned;

        // event to propogate upwards when the mechanical part of the garage has finished
        // processing a vehicle and is waiting for another RFID tag
        public event Action VehicleProcessed
        {
            add { MediatorProcessingState.VehicleProcessed += value; }
            remove { MediatorProcessingState.VehicleProcessed -= value; }
        }

        // request to change state is propogated downwards into the MediatorState
        public void Request()
        {
            State.Change(this);
        }
    }
}