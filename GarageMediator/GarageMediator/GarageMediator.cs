using System;
using GarageModel;
using SerialCommunications;
namespace GarageMediator
{
    public class GarageMediator
    {
        public MediatorState State { get; set; }

        public GarageMediator()
        {
            State = new MediatorReadyState();
        }

        public void Request()
        {
            State.Change(this);
        }
    }
}