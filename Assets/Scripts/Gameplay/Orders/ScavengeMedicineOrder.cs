using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class ScavengeMedicineOrder : IOrder
    {
        const string Narrative = "Two volunteers slip through the breach at dawn. By nightfall, crates arrive. The volunteers do not.";
        const double MedicineGain = 20;
        const double SicknessIncrease = 5;
        const double MedicineThreshold = 15;
        const int Deaths = 2;

        readonly IPopupService _popup;

        public ScavengeMedicineOrder(IPopupService popup) => _popup = popup;

        public string Id => "scavenge_medicine";
        public string Name => "Scavenge Medicine";
        public string Description => "Send workers beyond the walls to scavenge for medicine. Some will not return.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Medicine < MedicineThreshold;

        public void OnExecute(GameState state, ChangeLog log)
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
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new ScavengeMedicineOrder(_popup);
    }
}
