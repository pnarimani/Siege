using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    public class MissionManager
    {
        readonly Dictionary<string, Mission> _missions = new();
        readonly List<Mission> _active = new();
        readonly ChangeLog _changeLog;

        public IReadOnlyList<Mission> ActiveMissions => _active;
        public IReadOnlyDictionary<string, Mission> AllMissions => _missions;

        public event Action<Mission, MissionOutcome> MissionCompleted;

        public MissionManager(ChangeLog changeLog)
        {
            _changeLog = changeLog;
            RegisterAll();
        }

        void RegisterAll()
        {
            Register(new ForageBeyondWalls());
            Register(new NightRaid());
            Register(new SearchAbandonedHomes());
            Register(new NegotiateBlackMarketeers());
            Register(new SabotageEnemySupplies());
            Register(new ScoutingMission());
            Register(new Sortie());
            Register(new RaidCivilianFarms());
            Register(new DiplomaticEnvoy());
            Register(new EngineerTunnels());
        }

        void Register(Mission mission)
        {
            _missions[mission.Id] = mission;
        }

        public bool CanLaunch(string missionId, GameState state)
        {
            return _missions.TryGetValue(missionId, out var mission)
                   && !mission.IsActive
                   && mission.CanLaunch(state);
        }

        public bool Launch(string missionId, GameState state)
        {
            if (!CanLaunch(missionId, state)) return false;

            var mission = _missions[missionId];

            if (mission.WorkerCost > 0)
            {
                state.HealthyWorkers -= mission.WorkerCost;
                _changeLog.Record("HealthyWorkers", -mission.WorkerCost, mission.Name + " (deployed)");
            }

            if (mission.GuardCost > 0)
            {
                state.Guards -= mission.GuardCost;
                _changeLog.Record("Guards", -mission.GuardCost, mission.Name + " (deployed)");
            }

            mission.IsActive = true;
            mission.DaysRemaining = mission.DurationDays;
            _active.Add(mission);
            return true;
        }

        public void AdvanceDay(GameState state)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var mission = _active[i];
                mission.DaysRemaining--;

                if (mission.DaysRemaining <= 0)
                {
                    var outcome = mission.Resolve(state, _changeLog);
                    ReturnSurvivors(mission, outcome, state);

                    if (outcome.Success)
                        state.ConsecutiveMissionSuccessDays++;
                    else
                        state.ConsecutiveMissionSuccessDays = 0;

                    mission.IsActive = false;
                    _active.RemoveAt(i);

                    MissionCompleted?.Invoke(mission, outcome);
                }
            }
        }

        void ReturnSurvivors(Mission mission, MissionOutcome outcome, GameState state)
        {
            // Workers/guards were removed from pools on Launch.
            // Deaths are NOT subtracted again in Resolve — only TotalDeaths/DeathsToday are updated there.
            // Here we return the survivors (deployed - dead - wounded) as healthy, and wounded as sick.

            if (mission.WorkerCost > 0)
            {
                int healthy = Math.Max(0, mission.WorkerCost - outcome.Deaths - outcome.Wounded);
                if (healthy > 0)
                {
                    state.HealthyWorkers += healthy;
                    _changeLog.Record("HealthyWorkers", healthy, mission.Name + " (returned)");
                }

                if (outcome.Wounded > 0)
                {
                    state.SickWorkers += outcome.Wounded;
                    _changeLog.Record("SickWorkers", outcome.Wounded, mission.Name + " (wounded)");
                }
            }

            if (mission.GuardCost > 0)
            {
                int healthy = Math.Max(0, mission.GuardCost - outcome.Deaths - outcome.Wounded);
                if (healthy > 0)
                {
                    state.Guards += healthy;
                    _changeLog.Record("Guards", healthy, mission.Name + " (returned)");
                }

                if (outcome.Wounded > 0)
                {
                    state.WoundedGuards += outcome.Wounded;
                    _changeLog.Record("WoundedGuards", outcome.Wounded, mission.Name + " (wounded)");
                }
            }
        }
    }
}
