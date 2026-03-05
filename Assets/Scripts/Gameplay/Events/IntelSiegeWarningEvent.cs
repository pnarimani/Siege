using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class IntelSiegeWarningEvent : IGameEvent
    {
        const int MinDay = 15;
        const int MinSiegeIntensity = 4;
        const float TriggerChance = 0.15f;
        const int InterceptGuardCost = 3;
        const double BraceIntegrityBonus = 10.0;

        bool _hasTriggered;

        public string Id => "intel_siege_warning";
        public string Name => "Siege Escalation Warning";
        public string Description => "Intelligence warns of an imminent siege escalation.";

        public EventResponse[] GetResponses(GameState state) => new[]
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

        public string GetNarrativeText(GameState state) =>
            "A captured messenger reveals plans for a major assault. " +
            "You have a narrow window to act.";

        public bool CanTrigger(GameState state)
        {
            if (_hasTriggered) return false;
            if (state.CurrentDay < MinDay) return false;
            if (state.SiegeIntensity < MinSiegeIntensity) return false;
            if (Random.value >= TriggerChance) return false;
            _hasTriggered = true;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            switch (responseIndex)
            {
                case 0:
                    int lost = System.Math.Min(InterceptGuardCost, state.Guards);
                    state.Guards -= lost;
                    state.TotalDeaths += 1;
                    state.DeathsToday += 1;
                    state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                    log.Record("Guards", -lost, Name);
                    log.Record("TotalDeaths", 1, Name);
                    log.Record("SiegeIntensity", -1, Name);
                    break;
                case 1:
                    ZoneId perimeter = state.ActivePerimeter;
                    double current = state.GetZoneIntegrity(perimeter);
                    state.SetZoneIntegrity(perimeter, System.Math.Min(100.0, current + BraceIntegrityBonus));
                    log.Record("ZoneIntegrity", BraceIntegrityBonus, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new IntelSiegeWarningEvent();
    }
}
