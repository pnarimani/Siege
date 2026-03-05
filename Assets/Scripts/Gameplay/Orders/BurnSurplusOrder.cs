using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class BurnSurplusOrder : IOrder
    {
        const string Narrative = "The pyre burns high. The stench of rot gives way to clean smoke. People breathe a little easier.";
        const double MaterialsCost = 10;
        const double SicknessReduction = 8;
        const double MoraleGain = 8;

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public BurnSurplusOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "burn_surplus";
        public string Name => "Burn Surplus";
        public string Description => "Burn contaminated materials to cleanse the area, reducing sickness and lifting spirits.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            _ledger.Has(ResourceType.Materials, MaterialsCost);

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Withdraw(ResourceType.Materials, MaterialsCost);
            log.Record("Materials", -MaterialsCost, Id);

            state.Sickness -= SicknessReduction;
            log.Record("Sickness", -SicknessReduction, Id);

            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new BurnSurplusOrder(_popup, _ledger);
    }
}
