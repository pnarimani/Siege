using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ScoutingMissionHandler : IMissionHandler
    {
        readonly ScoutingMission _mission;
        readonly IPopupService _popup;

        const int Workers = 2;
        const float ChanceSuccess = 0.60f;
        const int FailDeaths = 3;
        const double FailUnrest = 15;

        public ScoutingMissionHandler(ScoutingMission mission, IPopupService popup)
        {
            _mission = mission;
            _popup = popup;
        }

        public string MissionId => _mission.Id;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= _mission.WorkerCost;

        public MissionOutcome Resolve(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceSuccess)
            {
                // Intel reveals enemy weak points — reduce siege intensity
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("SiegeIntensity", -1, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The scouts mapped the enemy camp. We know where they are weakest.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += FailUnrest;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Unrest", FailUnrest, _mission.Name);
            log.Record("Deaths", FailDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "The scouts were spotted. Their heads were catapulted over the walls.",
                Success = false,
                Deaths = FailDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
