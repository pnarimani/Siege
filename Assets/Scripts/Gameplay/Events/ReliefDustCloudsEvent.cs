using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class ReliefDustCloudsEvent : GameEvent
    {

        public bool HasTriggered { get; set; }
        public string Id => "relief_dust_clouds";
        public string Name => "Dust Clouds on the Horizon";
        public string Description => "Scouts report dust clouds to the east.";
        public int Priority => 95;

        public string GetNarrativeText(GameState state) =>
            "Scouts on the watchtower report dust clouds to the east. " +
            "Could be a caravan. Could be an army. Could be hope.";
    }
}
