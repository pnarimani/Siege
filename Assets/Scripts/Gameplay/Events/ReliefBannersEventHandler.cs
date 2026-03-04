using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEventHandler : IEventHandler
    {
        readonly ReliefBannersEvent _event;

        public string EventId => _event.Id;

        const int DaysBeforeRelief = 1;
        readonly ReliefArmy _reliefArmy;

        public ReliefBannersEventHandler(ReliefBannersEvent gameEvent, ReliefArmy reliefArmy)
        {
            _event = gameEvent;
            _reliefArmy = reliefArmy;
        }

        public bool CanTrigger(GameState state) =>
            _reliefArmy.ArrivalDay > 0 && state.CurrentDay == _reliefArmy.ArrivalDay - DaysBeforeRelief;
    }
}
