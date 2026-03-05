using System.Collections.Generic;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class ScavengingSystem : ISimulationSystem
    {
        const int LocationsPerNight = 4;
        const int TemplateCount = 8;

        readonly ChangeLog _changeLog;
        readonly ResourceLedger _ledger;
        readonly List<ScavengingLocation> _available = new();

        public IReadOnlyList<ScavengingLocation> AvailableLocations => _available;

        public ScavengingSystem(ChangeLog changeLog, ResourceLedger ledger)
        {
            _changeLog = changeLog;
            _ledger = ledger;
        }

        public void Tick(GameState state, float deltaTime) { }

        public void OnNightStart(GameState state, int day)
        {
            GenerateLocations();
        }

        void GenerateLocations()
        {
            _available.Clear();
            var used = new HashSet<int>();

            while (_available.Count < LocationsPerNight && used.Count < TemplateCount)
            {
                int index = Random.Range(0, TemplateCount);
                if (used.Add(index))
                    _available.Add(CreateFromTemplate(index));
            }
        }

        ScavengingLocation CreateFromTemplate(int index)
        {
            return index switch
            {
                0 => new ScavengingLocation
                {
                    Name = "Ruined Granary",
                    Danger = DangerLevel.Low,
                    MaxVisits = 3,
                    VisitsRemaining = 3,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Food, 20),
                        new ResourceQuantity(ResourceType.Food, 40)
                    },
                    CasualtyChance = 0.10f,
                    MaxCasualties = 1
                },
                1 => new ScavengingLocation
                {
                    Name = "Collapsed Warehouse",
                    Danger = DangerLevel.Medium,
                    MaxVisits = 2,
                    VisitsRemaining = 2,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Materials, 15),
                        new ResourceQuantity(ResourceType.Materials, 35)
                    },
                    CasualtyChance = 0.25f,
                    MaxCasualties = 2
                },
                2 => new ScavengingLocation
                {
                    Name = "Abandoned Apothecary",
                    Danger = DangerLevel.Low,
                    MaxVisits = 2,
                    VisitsRemaining = 2,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Medicine, 10),
                        new ResourceQuantity(ResourceType.Medicine, 25)
                    },
                    CasualtyChance = 0.05f,
                    MaxCasualties = 1
                },
                3 => new ScavengingLocation
                {
                    Name = "Flooded Cistern",
                    Danger = DangerLevel.Medium,
                    MaxVisits = 3,
                    VisitsRemaining = 3,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Water, 15),
                        new ResourceQuantity(ResourceType.Water, 35)
                    },
                    CasualtyChance = 0.20f,
                    MaxCasualties = 1
                },
                4 => new ScavengingLocation
                {
                    Name = "Burned-Out Manor",
                    Danger = DangerLevel.High,
                    MaxVisits = 1,
                    VisitsRemaining = 1,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Food, 30),
                        new ResourceQuantity(ResourceType.Materials, 20)
                    },
                    CasualtyChance = 0.40f,
                    MaxCasualties = 3
                },
                5 => new ScavengingLocation
                {
                    Name = "Woodcutter's Camp",
                    Danger = DangerLevel.Low,
                    MaxVisits = 3,
                    VisitsRemaining = 3,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Fuel, 15),
                        new ResourceQuantity(ResourceType.Fuel, 30)
                    },
                    CasualtyChance = 0.10f,
                    MaxCasualties = 1
                },
                6 => new ScavengingLocation
                {
                    Name = "Enemy Supply Cache",
                    Danger = DangerLevel.High,
                    MaxVisits = 1,
                    VisitsRemaining = 1,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Food, 40),
                        new ResourceQuantity(ResourceType.Water, 30)
                    },
                    CasualtyChance = 0.50f,
                    MaxCasualties = 3
                },
                7 => new ScavengingLocation
                {
                    Name = "Shattered Church",
                    Danger = DangerLevel.Medium,
                    MaxVisits = 2,
                    VisitsRemaining = 2,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Medicine, 10),
                        new ResourceQuantity(ResourceType.Materials, 15)
                    },
                    CasualtyChance = 0.20f,
                    MaxCasualties = 2
                },
                _ => new ScavengingLocation
                {
                    Name = "Rubble Pile",
                    Danger = DangerLevel.Low,
                    MaxVisits = 1,
                    VisitsRemaining = 1,
                    PossibleRewards = new[]
                    {
                        new ResourceQuantity(ResourceType.Materials, 5),
                        new ResourceQuantity(ResourceType.Materials, 15)
                    },
                    CasualtyChance = 0.05f,
                    MaxCasualties = 1
                }
            };
        }

        /// <summary>
        /// Send scavengers to a location. Returns narrative text describing the outcome.
        /// PossibleRewards[0] = min, PossibleRewards[1] = max for the primary reward.
        /// </summary>
        public string SendScavengers(int locationIndex, GameState state)
        {
            if (locationIndex < 0 || locationIndex >= _available.Count)
                return "Invalid location.";

            var location = _available[locationIndex];
            if (location.VisitsRemaining <= 0)
                return "This location has been picked clean.";

            location.VisitsRemaining--;

            // Roll for casualties
            int casualties = 0;
            if (Random.value < location.CasualtyChance)
            {
                casualties = Random.Range(1, location.MaxCasualties + 1);
                state.HealthyWorkers -= casualties;
                state.TotalDeaths += casualties;
                state.DeathsToday += casualties;
                _changeLog.Record("Deaths", casualties, "Scavenging: " + location.Name);
            }

            // Roll for rewards (each reward entry is min/max pair)
            for (int i = 0; i < location.PossibleRewards.Length; i += 2)
            {
                var min = location.PossibleRewards[i];
                int maxIndex = i + 1 < location.PossibleRewards.Length ? i + 1 : i;
                var max = location.PossibleRewards[maxIndex];

                double amount = min.Quantity + Random.value * (max.Quantity - min.Quantity);
                amount = System.Math.Round(amount);
                _ledger.Deposit(min.Resource, amount);
                _changeLog.Record(min.Resource.ToString(), amount, "Scavenging: " + location.Name);
            }

            if (casualties > 0)
                return $"Scavengers returned from {location.Name} but lost {casualties} to danger.";

            return $"Scavengers returned safely from {location.Name} with supplies.";
        }
    }
}
