using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EventDispatcher
    {
        readonly List<IGameEvent> _templates;
        readonly ChangeLog _changeLog;

        IGameEvent _pendingClone;

        public IGameEvent PendingEvent => _pendingClone;
        public event Action<IGameEvent> EventTriggered;

        public IReadOnlyList<IGameEvent> AllEvents => _templates;

        public EventDispatcher(IEnumerable<IGameEvent> events, ChangeLog changeLog)
        {
            _templates = new List<IGameEvent>(events);
            _changeLog = changeLog;
        }

        public void EvaluateEvents(GameState state)
        {
            if (_pendingClone != null) return;

            foreach (var template in _templates)
            {
                if (!template.CanTrigger(state)) continue;

                var clone = template.Clone();

                if (clone.GetResponses(state).Length > 0)
                {
                    _pendingClone = clone;
                    EventTriggered?.Invoke(clone);
                }
                else
                {
                    clone.Execute(state, _changeLog);
                    EventTriggered?.Invoke(clone);
                }

                return;
            }
        }

        public void RespondToEvent(GameState state, int responseIndex)
        {
            if (_pendingClone == null) return;
            _pendingClone.ExecuteResponse(state, _changeLog, responseIndex);
            _pendingClone = null;
        }

        public void DismissEvent()
        {
            _pendingClone = null;
        }
    }
}
