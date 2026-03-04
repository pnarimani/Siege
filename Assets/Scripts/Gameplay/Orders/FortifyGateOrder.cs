using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class FortifyGateOrder : Order
    {
        const int Cooldown = 3;
        const double MaterialsCost = 8;
        const double IntegrityGain = 10;
        const double UnrestIncrease = 3;
        const double IntegrityThreshold = 70;

        public override string Id => "fortify_gate";
        public override string Name => "Fortify Gate";
        public override string Description => "Reinforce the gate when defenses are weakened.";
        public override string NarrativeText => "Iron and timber are hammered into the gate. It groans but holds.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost
            && state.Zones[state.ActivePerimeter].Integrity < IntegrityThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
