using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class DiplomaticEnvoy : IMission
    {
        readonly IPopupService _popup;

        const int Duration = 3;
        const int Workers = 3;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 3;
        const double FailUnrest = 10;

        const string GreatText = "The envoys bought time. Rumors say a relief force stirs in the east.";
        const string PartialText = "The enemy entertained our envoys, if only for the amusement. A small delay.";
        const string FailText = "The envoys were hanged from the siege towers. A grim message.";

        int _daysRemaining;
        int _totalDuration;
        int _workersSent;

        public DiplomaticEnvoy(IPopupService popup) => _popup = popup;

        public string Id => "diplomatic_envoy";
        public string Name => "Diplomatic Envoy";
        public string Description => "Send envoys to seek terms or stall the enemy. Duration: 3d | Workers: 3";
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
                state.SiegeDamageReductionMultiplier = 0.7;
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome { NarrativeText = GreatText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                state.SiegeDamageReductionDays = 2;
                state.SiegeDamageReductionMultiplier = 0.85;
                log.Record("SiegeIntensity", 0, Name + " (delay)");
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

        public IMission Clone() => new DiplomaticEnvoy(_popup);

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
