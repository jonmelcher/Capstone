

namespace GarageMediator
{
    public class MediatorKilledState : MediatorState
    {
        public override void Change(GarageMediator context)
        {
            context.State = new MediatorReadyState();
        }
    }
}