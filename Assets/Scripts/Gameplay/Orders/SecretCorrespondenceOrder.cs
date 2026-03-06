using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class SecretCorrespondenceOrder : IOrder
    {
        const string Narrative = "A bird lands on the tower at dawn, a coded message bound to its leg. Hope is a fragile thing.";

        readonly IPopupService _popup;
        readonly PoliticalState _political;
        readonly ResourceLedger _ledger;

        public SecretCorrespondenceOrder(IPopupService popup, PoliticalState political, ResourceLedger ledger)
        {
            _popup = popup;
            _political = political;
            _ledger = ledger;
        }

        public string Id => "secret_correspondence";
        public string Name => "Secret Correspondence";
        public string Description => "Maintain covert communication with allies outside the walls. May yield supplies or hasten relief.";
        public int CooldownDays => 0;

        public bool CanIssue(GameState state) =>
            _political.Faith.Value >= 4;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new SecretCorrespondenceOrder(_popup, _political, _ledger);
    }
}
