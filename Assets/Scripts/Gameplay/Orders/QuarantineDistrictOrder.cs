using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class QuarantineDistrictOrder : Order
    {
        const int Cooldown = 3;
        const double SicknessReduction = 12;
        const double UnrestReduction = 3;
        const double SicknessThreshold = 30;

        public override string Id => "quarantine_district";
        public override string Name => "Quarantine District";
        public override string Description => "Seal off a diseased area to contain the outbreak.";
        public override string NarrativeText => "The barricades go up. Behind them, the coughing continues — but it does not spread.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Unrest -= UnrestReduction;
            log.Record("Unrest", -UnrestReduction, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
