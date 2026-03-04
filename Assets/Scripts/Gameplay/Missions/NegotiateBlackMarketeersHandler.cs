using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class NegotiateBlackMarketeersHandler : IMissionHandler
    {
        readonly NegotiateBlackMarketeers _mission;
        readonly IPopupService _popup;

        const int Workers = 2;
        const float ChanceWater = 0.45f;
        const float ChanceFood = 0.30f;
        const double WaterGain = 60;
        const double FoodGain = 50;
        const double DealUnrest = 10;
        const int BetrayalDeaths = 2;
        const double BetrayalUnrest = 25;

        public NegotiateBlackMarketeersHandler(NegotiateBlackMarketeers mission, IPopupService popup)
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

            if (roll < ChanceWater)
            {
                state.AddResource(ResourceType.Water, WaterGain);
                state.Unrest += DealUnrest;
                log.Record("Water", WaterGain, _mission.Name);
                log.Record("Unrest", DealUnrest, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "The smugglers delivered water barrels. The people mutter about favoritism.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceWater + ChanceFood)
            {
                state.AddResource(ResourceType.Food, FoodGain);
                state.Unrest += DealUnrest;
                log.Record("Food", FoodGain, _mission.Name);
                log.Record("Unrest", DealUnrest, _mission.Name);
                outcome = new MissionOutcome
                {
                    NarrativeText = "Grain sacks arrived in the night. Whispers spread about where they came from.",
                    Success = true
                };
                _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Unrest += BetrayalUnrest;
            state.TotalDeaths += BetrayalDeaths;
            state.DeathsToday += BetrayalDeaths;
            log.Record("Unrest", BetrayalUnrest, _mission.Name);
            log.Record("Deaths", BetrayalDeaths, _mission.Name);

            outcome = new MissionOutcome
            {
                NarrativeText = "It was a trap. The smugglers sold us out to the enemy.",
                Success = false,
                Deaths = BetrayalDeaths
            };
            _popup.Open(_mission.Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }
    }
}
