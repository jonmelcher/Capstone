

using GarageModel;
using SerialCommunications;


namespace GarageMediator
{
    public class MediatorReadyState : MediatorState
    {
        public MediatorReadyState()
        {
            DatabaseCommunication = GarageRepository.Instance;
            MicroCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.RS232) as RS232Server;
            RFIDCommunication = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;
        }

        public override void Change(GarageMediator context)
        {
            MicroCommunication.StartServer();
            RFIDCommunication.StartServer();
            context.State = new MediatorListeningState();
        }
    }
}