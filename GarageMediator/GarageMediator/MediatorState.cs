

using GarageModel;
using SerialCommunications;
using System;


namespace GarageMediator
{
    public abstract class MediatorState
    {
        protected static GarageRepository DatabaseCommunication { get; set; }
        protected static RS232Server MicroCommunication { get; set; }
        protected static Parallax28140Server RFIDCommunication { get; set; }

        public abstract void Change(GarageMediator context);

        public virtual void Process(GarageAssignment assignment)
        {
            throw new InvalidOperationException();
        }

        public virtual void Kill(GarageMediator context)
        {
            MicroCommunication?.StopServer();
            RFIDCommunication?.StopServer();
            context.State = new MediatorKilledState();
        }
    }
}