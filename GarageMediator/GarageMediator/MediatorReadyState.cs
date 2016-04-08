namespace GarageMediator
{
    public class MediatorReadyState : MediatorState
    {
        public override void Change(GarageMediator context)
        {
            context.MicroCommunication.StartServer();
            context.RFIDCommunication.StartServer();
            context.State = new MediatorListeningState(context);
        }
    }
}