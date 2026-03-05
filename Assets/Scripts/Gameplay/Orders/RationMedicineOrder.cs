using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class RationMedicineOrder : IOrder
    {
        const string Narrative = "The sick line up in the cold, clutching their numbered tokens. There is never enough.";
        const double MedicineCost = 8;
        const double SicknessReduction = 15;
        const double UnrestIncrease = 5;
        const double SicknessThreshold = 20;

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public RationMedicineOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "ration_medicine";
        public string Name => "Ration Medicine";
        public string Description => "Distribute medicine rations to the sick, reducing sickness at the cost of unrest.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Sickness > SicknessThreshold && _ledger.Has(ResourceType.Medicine, MedicineCost);

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Withdraw(ResourceType.Medicine, MedicineCost);
            log.Record("Medicine", -MedicineCost, Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new RationMedicineOrder(_popup, _ledger);
    }
}
