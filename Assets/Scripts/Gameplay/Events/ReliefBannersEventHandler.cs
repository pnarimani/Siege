using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEventHandler : EventHandler<ReliefBannersEvent>
    {
        const int DaysBeforeRelief = 1;
        readonly ReliefArmy _reliefArmy;

        public ReliefBannersEventHandler(ReliefBannersEvent gameEvent, ReliefArmy reliefArmy) : base(gameEvent)
        {
            _reliefArmy = reliefArmy;
        }

        public override bool CanTrigger(GameState state) =>
            _reliefArmy.ArrivalDay > 0 && state.CurrentDay == _reliefArmy.ArrivalDay - DaysBeforeRelief;
    }
}
