using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEvent : IGameEvent
    {
        const int DaysBeforeRelief = 7;

        readonly ReliefArmy _reliefArmy;
        bool _hasTriggered;

        public string Id => "relief_dust_clouds";
        public string Name => "Dust Clouds on the Horizon";
        public string Description => "Scouts report dust clouds to the east.";

        public ReliefDustCloudsEvent(ReliefArmy reliefArmy)
        {
            _reliefArmy = reliefArmy;
        }

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (_reliefArmy.ArrivalDay <= 0 || state.CurrentDay != _reliefArmy.ArrivalDay - DaysBeforeRelief)
                return false;

            _hasTriggered = true;
            return true;
        }

        public string GetNarrativeText(GameState state) =>
            "Scouts on the watchtower report dust clouds to the east. " +
            "Could be a caravan. Could be an army. Could be hope.";

        public IGameEvent Clone() => new ReliefDustCloudsEvent(_reliefArmy);
    }
}
