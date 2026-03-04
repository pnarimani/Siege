using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEventHandler : EventHandler<ReliefDustCloudsEvent>
    {
        const int DaysBeforeRelief = 7;

        public ReliefDustCloudsEventHandler(ReliefDustCloudsEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;
    }
}
