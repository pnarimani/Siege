using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEvent : GameEvent
    {
        const int DaysBeforeRelief = 7;

        public override string Id => "relief_dust_clouds";
        public override string Name => "Dust Clouds on the Horizon";
        public override string Description => "Scouts report dust clouds to the east.";
        public override int Priority => 95;

        public override bool CanTrigger(GameState state) =>
            state.ReliefArmyDay > 0 && state.CurrentDay == state.ReliefArmyDay - DaysBeforeRelief;

        public override string GetNarrativeText(GameState state) =>
            "Scouts on the watchtower report dust clouds to the east. " +
            "Could be a caravan. Could be an army. Could be hope.";
    }
}
