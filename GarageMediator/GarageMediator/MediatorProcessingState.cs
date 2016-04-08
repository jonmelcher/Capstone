

using System;
using System.Threading.Tasks;
using System.Threading;
using GarageModel;


namespace GarageMediator
{
    public class MediatorProcessingState : MediatorState
    {
        private const byte START_INSTRUCTION = 0xF0;
        private const byte CONTINUE_INSTRUCTION = 0xF1;
        private const byte STOP_INSTRUCTION = 0xF2;
        private const byte INSTRUCTIONS_COMPLETED = 0xF3;
        private const byte COMMUNICATION_DELAY_MS = 250;
        private const byte OUTGOING = 0x20;
        private const byte INCOMING = 0x21;

        private Task _instructor;
        public static event Action VehicleProcessed;
        
        public override void Process(GarageAssignment assignment)
        {
            _instructor = Task.Run(() =>
            {
                MicroCommunication.Write(START_INSTRUCTION);
                while (MicroCommunication.Read() != CONTINUE_INSTRUCTION)
                    Thread.Sleep(0);
                MicroCommunication.Write(assignment.Cell);
                while (MicroCommunication.Read() != CONTINUE_INSTRUCTION)
                    Thread.Sleep(0);
                MicroCommunication.Write(assignment.Stored ? OUTGOING : INCOMING);
                while (MicroCommunication.Read() != CONTINUE_INSTRUCTION)
                    Thread.Sleep(0);
                MicroCommunication.Write(STOP_INSTRUCTION);
                while (MicroCommunication.Read() != INSTRUCTIONS_COMPLETED)
                    Thread.Sleep(0);
                DatabaseCommunication.MoveVehicle(assignment.ID, !assignment.Stored);
                VehicleProcessed();
            });
        }

        public override void Kill(GarageMediator context)
        {
            _instructor?.Wait();
            base.Kill(context);
        }

        public override void Change(GarageMediator context)
        {
            context.State = new MediatorListeningState();
        }
    }
}