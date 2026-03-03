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
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 5;
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -1, Name);
                return new MissionOutcome
                {
                    NarrativeText = "The envoys bought time. Rumors say a relief force stirs in the east.",
                    Success = true
                };
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                log.Record("SiegeIntensity", 0, Name + " (delay)");
                state.SiegeDamageReductionDays = 2;
                state.SiegeDamageReductionMultiplier = 0.85;
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
