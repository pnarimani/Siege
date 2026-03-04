using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallBreachEventHandler : IEventHandler
    {
        const double IntegrityBreachThreshold = 30;
        const int GuardsToHold = 15;
        const int FailedDefenseDamage = 8;
        const int BarricadeMaterialCost = 10;
        const int BarricadeDamage = 5;
        const int AbandonDamage = 15;

        readonly WallBreachEvent _event;

        public string EventId => _event.Id;

        public WallBreachEventHandler(WallBreachEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.Zones[state.ActivePerimeter].Integrity < IntegrityBreachThreshold;

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var zone = state.ActivePerimeter;

            switch (responseIndex)
            {
                case 0:
                    if (state.Guards >= GuardsToHold)
                    {
                        log.Record("ZoneIntegrity:" + zone, 0, _event.Name + " (held)");
                    }
                    else
                    {
                        state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - FailedDefenseDamage);
                        log.Record("ZoneIntegrity:" + zone, -FailedDefenseDamage, _event.Name);
                    }
                    break;

                case 1:
                    state.Materials = Math.Max(0, state.Materials - BarricadeMaterialCost);
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - BarricadeDamage);
                    log.Record("Materials", -BarricadeMaterialCost, _event.Name);
                    log.Record("ZoneIntegrity:" + zone, -BarricadeDamage, _event.Name);
                    break;

                case 2:
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - AbandonDamage);
                    log.Record("ZoneIntegrity:" + zone, -AbandonDamage, _event.Name);
                    break;
            }
        }
    }
}
