using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class Sortie : IMission
    {
        readonly IPopupService _popup;

        const int DurationDays = 1;
        const int GuardsCost = 8;
        const float ChanceGreatSuccess = 0.40f;
        const float ChancePartialSuccess = 0.30f;
        const int FailDeaths = 4;
        const double FailUnrest = 20;

        const string GreatText = "A glorious charge! The enemy lines buckle and their siege stalls.";
        const string PartialText = "The sortie held them at bay. The enemy regroups cautiously.";
        const string FailText = "The sortie was crushed. Survivors limped back, and the city despairs.";

        int _daysRemaining;
        int _guardsSent;

        public Sortie(IPopupService popup) => _popup = popup;

        public string Id => "sortie";
        public string Name => "Sortie";
        public string Description => "Lead guards in a direct assault on enemy positions. Duration: 1d | Guards: 8";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => 1f - (float)_daysRemaining / DurationDays;

        public bool CanLaunch(GameState state) => state.Guards >= GuardsCost;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _daysRemaining = DurationDays;
            _guardsSent = GuardsCost;
            state.Guards -= GuardsCost;
            log.Record("Guards", -GuardsCost, Name);
        }

        public void AdvanceDay(GameState state, ChangeLog log) => _daysRemaining--;

        public MissionOutcome Complete(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceGreatSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
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
            state.Guards += _guardsSent;
            log.Record("Guards", _guardsSent, Name + " (cancelled)");
        }

        public IMission Clone() => new Sortie(_popup);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = System.Math.Max(0, _guardsSent - outcome.Deaths - outcome.Wounded);
            if (healthy > 0)
            {
                state.Guards += healthy;
                log.Record("Guards", healthy, Name + " (returned)");
            }
            if (outcome.Wounded > 0)
            {
                state.WoundedGuards += outcome.Wounded;
                log.Record("WoundedGuards", outcome.Wounded, Name + " (wounded)");
            }
        }
    }
}
