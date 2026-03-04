using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEventHandler : IEventHandler
    {
        readonly ReliefDustCloudsEvent _event;

        public string EventId => _event.Id;

        const int DaysBeforeRelief = 7;
        readonly ReliefArmy _reliefArmy;

        public ReliefDustCloudsEventHandler(ReliefDustCloudsEvent gameEvent, ReliefArmy reliefArmy)
        {
            _event = gameEvent;
            _reliefArmy = reliefArmy;
        }

        public bool CanTrigger(GameState state) =>
            _reliefArmy.ArrivalDay > 0 && state.CurrentDay == _reliefArmy.ArrivalDay - DaysBeforeRelief;
    }
}
