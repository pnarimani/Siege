using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefBannersEventHandler : EventHandler<ReliefBannersEvent>
    {
        const int DaysBeforeRelief = 1;

        public ReliefBannersEventHandler(ReliefBannersEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;
    }
}
