using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ScoutingMission : IMission
    {
        readonly IPopupService _popup;

        const int DurationDays = 2;
        const int Workers = 2;
        const float ChanceSuccess = 0.60f;
        const int FailDeaths = 3;
        const double FailUnrest = 15;

        const string SuccessText = "The scouts mapped the enemy camp. We know where they are weakest.";
        const string FailText = "The scouts were spotted. Their heads were catapulted over the walls.";

        int _daysRemaining;
        int _workersSent;

        public ScoutingMission(IPopupService popup) => _popup = popup;

        public string Id => "scouting_mission";
        public string Name => "Scouting Mission";
        public string Description => "Send scouts to gather intelligence on enemy positions. Duration: 2d | Workers: 2";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => 1f - (float)_daysRemaining / DurationDays;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _daysRemaining = DurationDays;
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

            if (roll < ChanceSuccess)
            {
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("SiegeIntensity", -1, Name);
                outcome = new MissionOutcome { NarrativeText = SuccessText, Success = true };
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

        public IMission Clone() => new ScoutingMission(_popup);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = System.Math.Max(0, _workersSent - outcome.Deaths - outcome.Wounded);
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
