using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public class BurnTheDeadLaw : ILaw
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const string Narrative = "The pyres burn day and night. The smoke chokes the living, but the plague recedes.";
        const double SicknessThreshold = 35;
        const double SicknessReduction = -15;
        const double FuelCost = -2;
        const double MoraleCost = -10;

        public BurnTheDeadLaw(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "burn_the_dead";
        public string Name => "Burn the Dead";
        public string Description => "Cremate the fallen to halt disease spread. Costs fuel and damages morale.";

        public bool CanEnact(GameState state) => state.Sickness > SicknessThreshold;

        public void OnEnact(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Sickness += SicknessReduction;
            log.Record("Sickness", SicknessReduction, "Burn the Dead");

            _ledger.Withdraw(ResourceType.Fuel, 2);
            log.Record("Fuel", FuelCost, "Burn the Dead");

            state.Morale += MoraleCost;
            log.Record("Morale", MoraleCost, "Burn the Dead");
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public ILaw Clone() => new BurnTheDeadLaw(_popup, _ledger);
    }
}
