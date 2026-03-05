using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class VoluntaryEvacuationOrder : IOrder
    {
        const string Narrative = "Families carry what they can. Behind them, the district falls silent.";
        const int MaxZonesLost = 4;

        readonly IPopupService _popup;

        public VoluntaryEvacuationOrder(IPopupService popup) => _popup = popup;

        public string Id => "voluntary_evacuation";
        public string Name => "Voluntary Evacuation";
        public string Description => "Abandon the outermost zone, pulling the perimeter inward.";
        public int CooldownDays => 0;

        public bool CanIssue(GameState state) =>
            state.ZonesLostCount < MaxZonesLost && state.ActivePerimeter != ZoneId.Keep;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            var zone = state.Zones[state.ActivePerimeter];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("ZoneLost", 1, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new VoluntaryEvacuationOrder(_popup);
    }
}
