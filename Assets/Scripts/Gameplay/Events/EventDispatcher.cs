using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    /// <summary>
    /// Wrapper class for all event operations. External systems interact with this, not individual handlers.
    /// Replaces EventManager.
    /// </summary>
    public class EventDispatcher
    {
        readonly List<IGameEvent> _events;
        readonly Dictionary<string, IEventHandler> _handlers;
        readonly ChangeLog _changeLog;

        public IGameEvent PendingEvent { get; private set; }
        public event Action<IGameEvent> EventTriggered;

        public IReadOnlyList<IGameEvent> AllEvents => _events;

        public EventDispatcher(IEnumerable<IGameEvent> events, IEnumerable<IEventHandler> handlers, ChangeLog changeLog)
        {
            _events = new List<IGameEvent>(events);
            _handlers = new Dictionary<string, IEventHandler>();
            foreach (var h in handlers) _handlers[h.EventId] = h;
            _changeLog = changeLog;
        }

        public void EvaluateEvents(GameState state)
        {
            if (PendingEvent != null) return;

            IGameEvent best = null;
            foreach (var e in _events)
            {
                if (e.IsOneTime && e.HasTriggered) continue;
                if (!_handlers.TryGetValue(e.Id, out var handler)) continue;
                if (!handler.CanTrigger(state)) continue;
                if (best == null || e.Priority > best.Priority) best = e;
            }

            if (best == null) return;

            best.HasTriggered = true;

            if (best.IsRespondable)
            {
                PendingEvent = best;
                EventTriggered?.Invoke(best);
            }
            else
            {
                if (_handlers.TryGetValue(best.Id, out var handler))
                    handler.Execute(state, _changeLog);
                EventTriggered?.Invoke(best);
            }
        }

        public void RespondToEvent(GameState state, int responseIndex)
        {
            if (PendingEvent == null) return;
            if (_handlers.TryGetValue(PendingEvent.Id, out var handler))
                handler.ExecuteResponse(state, _changeLog, responseIndex);
            PendingEvent = null;
        }

        public void DismissEvent()
        {
            PendingEvent = null;
        }
    }
}
