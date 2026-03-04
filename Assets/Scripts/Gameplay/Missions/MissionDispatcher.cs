using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    /// <summary>
    /// Wrapper class for all mission operations. External systems interact with this, not individual handlers.
    /// Replaces MissionManager.
    /// </summary>
    public class MissionDispatcher
    {
        readonly Dictionary<string, IMission> _missions;
        readonly Dictionary<string, IMissionHandler> _handlers;
        readonly List<IMission> _active = new();
        readonly ChangeLog _changeLog;

        public IReadOnlyList<IMission> ActiveMissions => _active;
        public IReadOnlyDictionary<string, IMission> AllMissions => _missions;

        public event Action<IMission, MissionOutcome> MissionCompleted;

        public MissionDispatcher(IEnumerable<IMission> missions, IEnumerable<IMissionHandler> handlers, ChangeLog changeLog)
        {
            _missions = new Dictionary<string, IMission>();
            foreach (var m in missions) _missions[m.Id] = m;
            _handlers = new Dictionary<string, IMissionHandler>();
            foreach (var h in handlers) _handlers[h.MissionId] = h;
            _changeLog = changeLog;
        }

        public bool CanLaunch(string id, GameState state)
        {
            return _missions.TryGetValue(id, out var mission)
                && _handlers.TryGetValue(id, out var handler)
                && !mission.IsActive
                && handler.CanLaunch(state);
        }

        public bool Launch(string id, GameState state)
        {
            if (!CanLaunch(id, state)) return false;

            var mission = _missions[id];

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
                    var handler = _handlers[mission.Id];
                    var outcome = handler.Resolve(state, _changeLog);
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

        void ReturnSurvivors(IMission mission, MissionOutcome outcome, GameState state)
        {
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
