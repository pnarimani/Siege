using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ForageBeyondWallsHandler : IMissionHandler
    {
        readonly ForageBeyondWalls _mission;
        readonly IPopupService _popup;

        const int Workers = 5;
        const float ChanceGreatSuccess = 0.50f;
        const float ChancePartialSuccess = 0.25f;
        const double GreatFood = 80;
        const double PartialFood = 50;
        const int AmbushDeaths = 2;
        const int AmbushWounded = 3;
        const double AmbushUnrest = 10;

        public ForageBeyondWallsHandler(ForageBeyondWalls mission, IPopupService popup)
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

            if (roll < ChanceGreatSuccess)
            {
                state.AddResource(ResourceType.Food, GreatFood);
                log.Record("Food", GreatFood, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The foragers found a hidden storehouse. A bounty of food returns to the city.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceGreatSuccess + ChancePartialSuccess)
            {
                state.AddResource(ResourceType.Food, PartialFood);
                log.Record("Food", PartialFood, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Slim pickings, but the foragers return with what they could carry.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += AmbushUnrest;
            state.TotalDeaths += AmbushDeaths;
            state.DeathsToday += AmbushDeaths;
            log.Record("Unrest", AmbushUnrest, _mission.Name);
            log.Record("Deaths", AmbushDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "An enemy patrol ambushed the foragers. Bodies were dragged back through the gate.",
                Success = false,
                Deaths = AmbushDeaths,
                Wounded = AmbushWounded
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
