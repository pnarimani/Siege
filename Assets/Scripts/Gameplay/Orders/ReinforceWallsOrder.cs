using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

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
            state.Materials >= MaterialsCost && Resolver.Resolve<PoliticalState>().Fortification.Value >= 2;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
