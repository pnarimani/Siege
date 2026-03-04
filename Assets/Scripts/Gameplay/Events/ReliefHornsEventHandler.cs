using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefHornsEventHandler : EventHandler<ReliefHornsEvent>
    {
        const int DaysBeforeRelief = 3;
        readonly ReliefArmy _reliefArmy;

        public ReliefHornsEventHandler(ReliefHornsEvent gameEvent, ReliefArmy reliefArmy) : base(gameEvent)
        {
            _reliefArmy = reliefArmy;
        }

        public override bool CanTrigger(GameState state) =>
            _reliefArmy.ArrivalDay > 0 && state.CurrentDay == _reliefArmy.ArrivalDay - DaysBeforeRelief;
    }
}
