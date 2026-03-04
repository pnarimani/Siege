using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEventHandler : EventHandler<ReliefDustCloudsEvent>
    {
        const int DaysBeforeRelief = 7;
        readonly ReliefArmy _reliefArmy;

        public ReliefDustCloudsEventHandler(ReliefDustCloudsEvent gameEvent, ReliefArmy reliefArmy) : base(gameEvent)
        {
            _reliefArmy = reliefArmy;
        }

        public override bool CanTrigger(GameState state) =>
            _reliefArmy.ArrivalDay > 0 && state.CurrentDay == _reliefArmy.ArrivalDay - DaysBeforeRelief;
    }
}
