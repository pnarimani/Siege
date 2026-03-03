using System;
using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Random = UnityEngine.Random;

namespace Siege.Gameplay.Events
{
    public class IntelSiegeWarningEvent : GameEvent
    {
        const int MinDay = 15;
        const int MinSiegeIntensity = 4;
        const float TriggerChance = 0.15f;
        const int InterceptGuardCost = 3;
        const double BraceIntegrityBonus = 10.0;

        public override string Id => "intel_siege_warning";
        public override string Name => "Siege Escalation Warning";
        public override string Description => "Intelligence warns of an imminent siege escalation.";
        public override int Priority => 55;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= MinDay &&
            state.SiegeIntensity >= MinSiegeIntensity &&
            Random.value < TriggerChance;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Intercept",
                "Send guards to intercept. Some will not return, but siege pressure eases.",
                "Your guards ambush the enemy's forward scouts under cover of night. " +
                "Not all return, but the enemy's advance falters."),
            new EventResponse(
                "Brace",
                "Reinforce the perimeter instead.",
                "Work crews shore up the walls through the night. " +
                "The perimeter holds a little stronger.")
        };

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
                    log.Record("Guards", -lost, Name);
                    log.Record("TotalDeaths", 1, Name);
                    log.Record("SiegeIntensity", -1, Name);
                    break;
                case 1:
                    ZoneId perimeter = state.ActivePerimeter;
                    double current = state.GetZoneIntegrity(perimeter);
                    state.SetZoneIntegrity(perimeter, Math.Min(100.0, current + BraceIntegrityBonus));
                    log.Record("ZoneIntegrity", BraceIntegrityBonus, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) =>
            "A captured messenger reveals plans for a major assault. " +
            "You have a narrow window to act.";
    }
}
