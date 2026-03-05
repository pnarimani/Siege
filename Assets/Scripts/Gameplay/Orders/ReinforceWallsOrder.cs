using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class ReinforceWallsOrder : IOrder
    {
        const string Narrative = "Workers haul rubble and timber through the night. The wall holds — for now.";
        const double MaterialsCost = 15;
        const double IntegrityGain = 15;

        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public ReinforceWallsOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "reinforce_walls";
        public string Name => "Reinforce Walls";
        public string Description => "Spend materials to shore up the perimeter walls.";
        public int CooldownDays => 3;

        public bool CanIssue(GameState state) =>
            state.Materials >= MaterialsCost && _political.Fortification.Value >= 2;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Materials -= MaterialsCost;
            log.Record("Materials", -MaterialsCost, Id);

            var zone = state.Zones[state.ActivePerimeter];
            zone.Integrity += IntegrityGain;
            log.Record("Integrity", IntegrityGain, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new ReinforceWallsOrder(_popup, _political);
    }
}
