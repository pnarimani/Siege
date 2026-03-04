using System;
using Siege.Gameplay.Simulation;
using Random = UnityEngine.Random;

namespace Siege.Gameplay.Events
{
    public class IntelSiegeWarningEventHandler : EventHandler<IntelSiegeWarningEvent>
    {
        const int MinDay = 15;
        const int MinSiegeIntensity = 4;
        const float TriggerChance = 0.15f;
        const int InterceptGuardCost = 3;
        const double BraceIntegrityBonus = 10.0;

        public IntelSiegeWarningEventHandler(IntelSiegeWarningEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= MinDay &&
            state.SiegeIntensity >= MinSiegeIntensity &&
            Random.value < TriggerChance;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    int lost = Math.Min(InterceptGuardCost, state.Guards);
                    state.Guards -= lost;
                    state.TotalDeaths += 1;
                    state.DeathsToday += 1;
                    state.SiegeIntensity = Math.Max(1, state.SiegeIntensity - 1);
                    log.Record("Guards", -lost, Event.Name);
                    log.Record("TotalDeaths", 1, Event.Name);
                    log.Record("SiegeIntensity", -1, Event.Name);
                    break;
                case 1:
                    ZoneId perimeter = state.ActivePerimeter;
                    double current = state.GetZoneIntegrity(perimeter);
                    state.SetZoneIntegrity(perimeter, Math.Min(100.0, current + BraceIntegrityBonus));
                    log.Record("ZoneIntegrity", BraceIntegrityBonus, Event.Name);
                    break;
            }
        }
    }
}
