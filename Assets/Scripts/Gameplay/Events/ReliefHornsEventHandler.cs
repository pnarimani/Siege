using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefHornsEventHandler : EventHandler<ReliefHornsEvent>
    {
        const int DaysBeforeRelief = 3;

        public ReliefHornsEventHandler(ReliefHornsEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;
    }
}
