using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class EngineerTunnels : IMission
    {
        readonly IPopupService _popup;

        const int Duration = 4;
        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 10;

        const string GreatText = "The tunnels collapsed their siege ramp. The enemy scrambles to rebuild.";
        const string PartialText = "The tunnels disrupted their approach. A partial success.";
        const string FailText = "The tunnels collapsed. Workers are buried alive.";

        int _daysRemaining;
        int _totalDuration;
        int _workersSent;

        public EngineerTunnels(IPopupService popup) => _popup = popup;

        public string Id => "engineer_tunnels";
        public string Name => "Engineer Tunnels";
        public string Description => "Dig counter-tunnels to undermine enemy siege works. Duration: 4d | Workers: 5";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => _totalDuration > 0 ? 1f - (float)_daysRemaining / _totalDuration : 0f;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _totalDuration = Duration;
            _daysRemaining = Duration;
            _workersSent = Workers;
            state.HealthyWorkers -= Workers;
            log.Record("HealthyWorkers", -Workers, Name);
        }

        public void AdvanceDay(GameState state, ChangeLog log) => _daysRemaining--;

        public MissionOutcome Complete(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = Mathf.Max(1, state.SiegeIntensity - 1);
                state.SiegeDamageReductionDays = 5;
                state.SiegeDamageReductionMultiplier = 0.6;
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome { NarrativeText = GreatText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                state.SiegeDamageReductionDays = 3;
                state.SiegeDamageReductionMultiplier = 0.8;
                log.Record("SiegeIntensity", 0, Name + " (partial)");
                outcome = new MissionOutcome { NarrativeText = PartialText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, Name);
            log.Record("Deaths", FailDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = FailDeaths };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new EngineerTunnels(_popup);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = Mathf.Max(0, _workersSent - outcome.Deaths - outcome.Wounded);
            if (healthy > 0)
            {
                state.HealthyWorkers += healthy;
                log.Record("HealthyWorkers", healthy, Name + " (returned)");
            }
            if (outcome.Wounded > 0)
            {
                state.SickWorkers += outcome.Wounded;
                log.Record("SickWorkers", outcome.Wounded, Name + " (wounded)");
            }
        }
    }
}
