using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class ForcedLaborOrder : IOrder
    {
        const string Narrative = "Under the lash, the rubble is cleared. Two bodies are pulled from the wreckage at dusk.";
        const double MaterialsGain = 15;
        const double UnrestIncrease = 8;
        const int Deaths = 2;

        readonly IPopupService _popup;
        readonly PoliticalState _political;
        readonly ResourceLedger _ledger;

        public ForcedLaborOrder(IPopupService popup, PoliticalState political, ResourceLedger ledger)
        {
            _popup = popup;
            _political = political;
            _ledger = ledger;
        }

        public string Id => "forced_labor";
        public string Name => "Forced Labor";
        public string Description => "Press civilians into dangerous labor to gather materials. Not all will survive.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            _political.Faith.Value < 4;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _ledger.Deposit(ResourceType.Materials, MaterialsGain);
            log.Record("Materials", MaterialsGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);

            state.HealthyWorkers -= Deaths;
            state.TotalDeaths += Deaths;
            state.DeathsToday += Deaths;
            log.Record("HealthyWorkers", -Deaths, Id);
            log.Record("Deaths", Deaths, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new ForcedLaborOrder(_popup, _political, _ledger);
    }
}
