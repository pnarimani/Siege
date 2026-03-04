using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ScoutingMission : Mission
    {
        const int Duration = 2;
        const int Workers = 2;
        const float ChanceSuccess = 0.60f;
        const int FailDeaths = 3;
        const double FailUnrest = 15;

        public override string Id => "scouting_mission";
        public override string Name => "Scouting Mission";
        public override string Description => "Send scouts to gather intelligence on enemy positions.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceSuccess)
            {
                // Intel reveals enemy weak points — reduce siege intensity
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The scouts mapped the enemy camp. We know where they are weakest.",
                    Success = true
                };
                Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Name);
            log.Record("Deaths", FailDeaths, Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The scouts were spotted. Their heads were catapulted over the walls.",
                Success = false,
                Deaths = FailDeaths
            };
            Popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
