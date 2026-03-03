using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class EngineerTunnels : Mission
    {
        const int Duration = 4;
        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 10;

        public override string Id => "engineer_tunnels";
        public override string Name => "Engineer Tunnels";
        public override string Description => "Dig counter-tunnels to undermine enemy siege works.";
        public override int DurationDays => Duration;
        public override int WorkerCost => Workers;

        public override bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public override MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            float roll = Random.value;

            if (roll < ChanceGreatSuccess)
            {
                // Siege damage ×0.6 for 5 days — immediate intensity drop as proxy
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("SiegeIntensity", -1, Name);
                // TODO: temporal 5-day siege damage ×0.6 modifier
                return new MissionOutcome
                {
                    NarrativeText = "The tunnels collapsed their siege ramp. The enemy scrambles to rebuild.",
                    Success = true
                };
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                // Siege damage ×0.8 for 3 days
                log.Record("SiegeIntensity", 0, Name + " (partial)");
                // TODO: temporal 3-day siege damage ×0.8 modifier
                return new MissionOutcome
                {
                    NarrativeText = "The tunnels disrupted their approach. A partial success.",
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
                NarrativeText = "The tunnels collapsed. Workers are buried alive.",
                Success = false,
                Deaths = FailDeaths
            };
        }
    }
}
