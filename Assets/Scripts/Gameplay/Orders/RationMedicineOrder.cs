using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class RationMedicineOrder : Order
    {
        const int Cooldown = 3;
        const double MedicineCost = 8;
        const double SicknessReduction = 15;
        const double UnrestIncrease = 5;
        const double SicknessThreshold = 20;

        public override string Id => "ration_medicine";
        public override string Name => "Ration Medicine";
        public override string Description => "Distribute medicine rations to the sick, reducing sickness at the cost of unrest.";
        public override string NarrativeText => "The sick line up in the cold, clutching their numbered tokens. There is never enough.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold && state.Medicine >= MedicineCost;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine -= MedicineCost;
            log.Record("Medicine", -MedicineCost, Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
