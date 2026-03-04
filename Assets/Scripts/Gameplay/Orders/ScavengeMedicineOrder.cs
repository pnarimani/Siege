using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ScavengeMedicineOrder : Order
    {
        const int Cooldown = 3;
        const double MedicineGain = 20;
        const double SicknessIncrease = 5;
        const double MedicineThreshold = 15;
        const int Deaths = 2;

        public override string Id => "scavenge_medicine";
        public override string Name => "Scavenge Medicine";
        public override string Description => "Send workers beyond the walls to scavenge for medicine. Some will not return.";
        public override string NarrativeText => "Two volunteers slip through the breach at dawn. By nightfall, crates arrive. The volunteers do not.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Medicine < MedicineThreshold;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Medicine += MedicineGain;
            log.Record("Medicine", MedicineGain, Id);

            state.Sickness += SicknessIncrease;
            log.Record("Sickness", SicknessIncrease, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
