

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

        public GarageMediator()
        {
            DatabaseCommunication = GarageRepository.Instance;
            MicroCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.RS232) as RS232Server;
            RFIDCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;
            MediatorListeningState.IDScanned += (id) => IDScanned(DatabaseCommunication.GetGarageAssignment(id),
                                                               DatabaseCommunication.GetVehicleInformation(id));
            State = new MediatorReadyState();
        }

        public event Action<GarageAssignment, VehicleInformation> IDScanned;
        public event Action VehicleProcessed
        {
            add { MediatorProcessingState.VehicleProcessed += value; }
            remove { MediatorProcessingState.VehicleProcessed -= value; }
        }

        public void Request()
        {
            State.Change(this);
        }
    }
}