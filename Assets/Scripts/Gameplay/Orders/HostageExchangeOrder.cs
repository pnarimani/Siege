using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class HostageExchangeOrder : IOrder
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const string Narrative = "A figure stumbles through the gate, gaunt and shaking. One more mouth to feed. One more soul saved.";

        public HostageExchangeOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "hostage_exchange";
        public string Name => "Hostage Exchange";
        public string Description => "Trade supplies to recover captured citizens. A slow, costly process.";
        public int CooldownDays => 0;

        public bool CanIssue(GameState state) =>
            state.ZonesLostCount >= 1;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new HostageExchangeOrder(_popup, _ledger);
    }
}
