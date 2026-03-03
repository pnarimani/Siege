using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class DiplomaticEnvoy : Mission
    {
        const int Duration = 3;
        const int Workers = 3;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int GreatDelay = 5;
        const int PartialDelay = 2;
        const int FailDeaths = 3;
        const double FailUnrest = 10;

        public override string Id => "diplomatic_envoy";
        public override string Name => "Diplomatic Envoy";
        public override string Description => "Send envoys to seek terms or stall the enemy.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            float roll = Random.value;

            if (roll < ChanceGreatSuccess)
            {
                // Major delay — may accelerate relief army arrival
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("SiegeIntensity", -1, Name);
                // TODO: temporal siege delay of +5 days; accelerate relief army
                return new MissionOutcome
                {
                    NarrativeText = "The envoys bought time. Rumors say a relief force stirs in the east.",
                    Success = true
                };
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                // Minor delay
                log.Record("SiegeIntensity", 0, Name + " (delay)");
                // TODO: temporal siege delay of +2 days
                return new MissionOutcome
                {
                    NarrativeText = "The enemy entertained our envoys, if only for the amusement. A small delay.",
                    Success = true
                };
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Name);
            log.Record("Deaths", FailDeaths, Name);

            return new MissionOutcome
            {
                NarrativeText = "The envoys were hanged from the siege towers. A grim message.",
                Success = false,
                Deaths = FailDeaths
            };
        }
    }
}
