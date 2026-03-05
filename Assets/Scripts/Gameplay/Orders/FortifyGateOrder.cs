using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class FortifyGateOrder : IOrder
    {
        readonly IPopupService _popup;

        const string Narrative = "Iron and timber are hammered into the gate. It groans but holds.";
        const double MaterialsCost = 8;
        const double IntegrityGain = 10;
        const double UnrestIncrease = 3;
        const double IntegrityThreshold = 70;

        public FortifyGateOrder(IPopupService popup) => _popup = popup;

        public string Id => "fortify_gate";
        public string Name => "Fortify Gate";
        public string Description => "Reinforce the gate when defenses are weakened.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost
            && state.Zones[state.ActivePerimeter].Integrity < IntegrityThreshold;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Id);

            state.Unrest += UnrestIncrease;
            log.Record("Unrest", UnrestIncrease, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new FortifyGateOrder(_popup);
    }
}
