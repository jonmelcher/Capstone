

using GarageModel;
using System;


namespace GarageMediator
{
    public abstract class MediatorState
    {
        public abstract void Change(GarageMediator context);

        public virtual void Process(GarageAssignment assignment)
        {
            throw new InvalidOperationException();
        }

        public virtual void Kill(GarageMediator context)
        {
            context.MicroCommunication?.StopServer();
            context.RFIDCommunication?.StopServer();
            context.State = new MediatorKilledState();
        }
    }
}