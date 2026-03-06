using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrder : IOrder
    {
        const string Narrative = "Gold changes hands in the dark. The bombardment eases — but someone may be watching.";

        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        public BribeEnemyOfficerOrder(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "bribe_enemy_officer";
        public string Name => "Bribe Enemy Officer";
        public string Description => "Pay a daily tribute to an enemy officer to reduce siege damage. Risk of interception.";
        public int CooldownDays => 0;

        public bool CanIssue(GameState state) => true;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new BribeEnemyOfficerOrder(_popup, _ledger);
    }
}
