using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NightRaid : IMission
    {
        readonly IPopupService _popup;

        const int Duration = 2;
        const int Workers = 6;
        const float ChanceGreatSuccess = 0.30f;
        const float ChancePartialSuccess = 0.40f;
        const int FailDeaths = 2;
        const int FailWounded = 4;
        const double FailUnrest = 15;

        const string GreatText = "The raid was devastating. Enemy siege engines burn and their advance stalls.";
        const string PartialText = "The raiders caused some disruption. The enemy pauses to regroup.";
        const string FailText = "The raid failed. Survivors stumbled back bloodied, and the city's fear grows.";

        int _daysRemaining;
        int _totalDuration;
        int _workersSent;

        public NightRaid(IPopupService popup) => _popup = popup;

        public string Id => "night_raid";
        public string Name => "Night Raid";
        public string Description => "Launch a daring night raid against enemy siege works. Duration: 2d | Workers: 6";
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
                state.SiegeDamageReductionDays = 3;
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
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = FailDeaths, Wounded = FailWounded };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new NightRaid(_popup);

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
