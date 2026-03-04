using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class VoluntaryEvacuationOrderHandler : OrderHandler<VoluntaryEvacuationOrder>
    {
        const int MaxZonesLost = 4;

        public VoluntaryEvacuationOrderHandler(VoluntaryEvacuationOrder order, IPopupService popup) : base(order, popup) { }

        public override bool CanIssue(GameState state) =>
            state.ZonesLostCount < MaxZonesLost && state.ActivePerimeter != ZoneId.Keep;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            // NOTE: simplified — real evacuation should go through ZoneManager
            var zone = state.Zones[state.ActivePerimeter];
            zone.IsLost = true;
            zone.Integrity = 0;
            log.Record("ZoneLost", 1, Order.Id);
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }
    }
}
