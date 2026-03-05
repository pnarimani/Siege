using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Missions
{
    public class MissionDispatcher
    {
        readonly List<IMission> _templates;
        readonly List<IMission> _active = new();
        readonly ChangeLog _changeLog;

        public IReadOnlyList<IMission> ActiveMissions => _active;
        public IReadOnlyList<IMission> AllMissions => _templates;

        public event Action<IMission, MissionOutcome> MissionCompleted;
        public event Action<string> MissionLaunched;

        public MissionDispatcher(IEnumerable<IMission> missions, ChangeLog changeLog)
        {
            _templates = new List<IMission>(missions);
            _changeLog = changeLog;
        }

        public IMission GetMission(string id)
        {
            foreach (var m in _templates)
                if (m.Id == id) return m;
            return null;
        }

        public bool IsActive(string id)
        {
            foreach (var m in _active)
                if (m.Id == id) return true;
            return false;
        }

        public bool CanLaunch(string id, GameState state)
        {
            var template = GetMission(id);
            return template != null && !IsActive(id) && template.CanLaunch(state);
        }

        public bool Launch(string id, GameState state)
        {
            if (!CanLaunch(id, state)) return false;

            var template = GetMission(id);
            var copy = template.Clone();
            copy.OnLaunch(state, _changeLog);
            _active.Add(copy);
            MissionLaunched?.Invoke(id);
            return true;
        }

        public void AdvanceDay(GameState state)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var mission = _active[i];
                mission.AdvanceDay(state, _changeLog);

                if (mission.IsComplete)
                {
                    var outcome = mission.Complete(state, _changeLog);

                    if (outcome.Success)
                        state.ConsecutiveMissionSuccessDays++;
                    else
                        state.ConsecutiveMissionSuccessDays = 0;

                    _active.RemoveAt(i);
                    MissionCompleted?.Invoke(mission, outcome);
                }
            }
        }
    }
}
