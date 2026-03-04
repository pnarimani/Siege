using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class VoluntaryEvacuationOrderHandler : IOrderHandler
    {
        const int MaxZonesLost = 4;

        readonly VoluntaryEvacuationOrder _order;
        readonly IPopupService _popup;

        public VoluntaryEvacuationOrderHandler(VoluntaryEvacuationOrder order, IPopupService popup)
        {
            _order = order;
            _popup = popup;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            state.ZonesLostCount < MaxZonesLost && state.ActivePerimeter != ZoneId.Keep;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            // NOTE: simplified — real evacuation should go through ZoneManager
            var zone = state.Zones[state.ActivePerimeter];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("ZoneLost", 1, _order.Id);
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }
    }
}
