using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Orders
{
    public class ReinforceWallsOrder : Order
    {
        const int Cooldown = 3;
        const double MaterialsCost = 15;
        const double IntegrityGain = 15;

        public override string Id => "reinforce_walls";
        public override string Name => "Reinforce Walls";
        public override string Description => "Spend materials to shore up the perimeter walls.";
        public override string NarrativeText => "Workers haul rubble and timber through the night. The wall holds — for now.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost; // TODO: require Fortification >= 2

        public override void Execute(GameState state, ChangeLog log)
        {
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Id);
        }
    }
}
