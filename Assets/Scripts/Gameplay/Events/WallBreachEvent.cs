using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallBreachEvent : IGameEvent
    {
        const double IntegrityBreachThreshold = 30;
        const int GuardsToHold = 15;
        const int FailedDefenseDamage = 8;
        const int BarricadeMaterialCost = 10;
        const int BarricadeDamage = 5;
        const int AbandonDamage = 15;

        readonly ResourceLedger _ledger;

        public WallBreachEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "wall_breach";
        public string Name => "Wall Breach";
        public string Description => "The perimeter wall groans and splits. The enemy will not wait long to exploit the gap.";

        public bool CanTrigger(GameState state) =>
            state.Zones[state.ActivePerimeter].Integrity < IntegrityBreachThreshold;

        public EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Reinforce the breach",
                    state.Guards >= GuardsToHold
                        ? "Guards hold the line — breach contained."
                        : "Not enough guards — the wall buckles further."),
                new EventResponse(
                    "Barricade with materials",
                    "-10 Materials, minor integrity loss."),
                new EventResponse(
                    "Fall back",
                    "Abandon the section — significant integrity loss.")
            };
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var zone = state.ActivePerimeter;

            switch (responseIndex)
            {
                case 0:
                    if (state.Guards >= GuardsToHold)
                    {
                        log.Record("ZoneIntegrity:" + zone, 0, Name + " (held)");
                    }
                    else
                    {
                        state.Zones[zone].Integrity = System.Math.Max(0, state.Zones[zone].Integrity - FailedDefenseDamage);
                        log.Record("ZoneIntegrity:" + zone, -FailedDefenseDamage, Name);
                    }
                    break;

                case 1:
                    _ledger.Withdraw(ResourceType.Materials, BarricadeMaterialCost);
                    state.Zones[zone].Integrity = System.Math.Max(0, state.Zones[zone].Integrity - BarricadeDamage);
                    log.Record("Materials", -BarricadeMaterialCost, Name);
                    log.Record("ZoneIntegrity:" + zone, -BarricadeDamage, Name);
                    break;

                case 2:
                    state.Zones[zone].Integrity = System.Math.Max(0, state.Zones[zone].Integrity - AbandonDamage);
                    log.Record("ZoneIntegrity:" + zone, -AbandonDamage, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new WallBreachEvent(_ledger);
    }
}
