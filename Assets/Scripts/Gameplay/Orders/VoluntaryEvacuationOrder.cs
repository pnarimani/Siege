using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class VoluntaryEvacuationOrder : Order
    {
        const int Cooldown = 0;
        const int MaxZonesLost = 4;

        public override string Id => "voluntary_evacuation";
        public override string Name => "Voluntary Evacuation";
        public override string Description => "Abandon the outermost zone, pulling the perimeter inward.";
        public override string NarrativeText => "Families carry what they can. Behind them, the district falls silent.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.ZonesLostCount < MaxZonesLost; // TODO: check zone can be evacuated

        public override void Execute(GameState state, ChangeLog log)
        {
            // NOTE: simplified — real evacuation should go through ZoneManager
            var zone = state.Zones[state.ActivePerimeter];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("ZoneLost", 1, Id);
        }
    }
}
