using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class WallBreachEventHandler : EventHandler<WallBreachEvent>
    {
        public WallBreachEventHandler(WallBreachEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.Zones[state.ActivePerimeter].Integrity < 30;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var zone = state.ActivePerimeter;

            switch (responseIndex)
            {
                case 0:
                    if (state.Guards >= 15)
                    {
                        log.Record("ZoneIntegrity:" + zone, 0, Event.Name + " (held)");
                    }
                    else
                    {
                        state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 8);
                        log.Record("ZoneIntegrity:" + zone, -8, Event.Name);
                    }
                    break;

                case 1:
                    state.Materials = Math.Max(0, state.Materials - 10);
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 5);
                    log.Record("Materials", -10, Event.Name);
                    log.Record("ZoneIntegrity:" + zone, -5, Event.Name);
                    break;

                case 2:
                    state.Zones[zone].Integrity = Math.Max(0, state.Zones[zone].Integrity - 15);
                    log.Record("ZoneIntegrity:" + zone, -15, Event.Name);
                    break;
            }
        }
    }
}
