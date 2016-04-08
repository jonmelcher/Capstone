

using System;
using GarageModel;


namespace GarageMediator
{
    public class GarageMediator
    {
        public MediatorState State { get; set; }

        public GarageMediator()
        {
            State = new MediatorReadyState();
        }

        public event Action VehicleProcessed
        {
            add { MediatorProcessingState.VehicleProcessed += value; }
            remove { MediatorProcessingState.VehicleProcessed -= value; }
        }

        public event Action<GarageAssignment, VehicleInformation> IDScanned
        {
            add { MediatorListeningState.IDScanned += value; }
            remove { MediatorListeningState.IDScanned -= value; }
        }

        public void Request()
        {
            State.Change(this);
        }
    }
}