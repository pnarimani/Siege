using System;
using System.Collections.Generic;
using Siege.Gameplay.Events;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.UI
{
    public class PopupService : IPopupService, IBackButtonHandler, IDisposable
    {
        readonly GameState _state;
        readonly GameClock _clock;
        readonly EventDispatcher _eventDispatcher;
        readonly BackButtonManager _backButtonManager;
        readonly Queue<DialogContent> _queue = new();

        GUIEventDialog _dialog;
        DialogContent _current;
        bool _wasPaused;

        public PopupService(
            GameState state,
            GameClock clock,
            EventDispatcher eventDispatcher,
            BackButtonManager backButtonManager)
        {
            _state = state;
            _clock = clock;
            _eventDispatcher = eventDispatcher;
            _backButtonManager = backButtonManager;
            _eventDispatcher.EventTriggered += OnEventTriggered;
        }

        public void Open(string title, string narrative, IReadOnlyList<StateChange> changes = null)
        {
            var content = new DialogContent
            {
                Title = title,
                Description = narrative,
                Changes = changes,
                OnDismiss = AdvanceQueue
            };

            Enqueue(content);
        }

        void OnEventTriggered(IGameEvent evt)
        {
            var responses = evt.GetResponses(_state);
            var content = new DialogContent
            {
                Title = evt.Name,
                Description = evt.Description, 
                OnDismiss = () =>
                {
                    _eventDispatcher.DismissEvent();
                    AdvanceQueue();
                }
            };

            if (responses.Length > 0)
            {
                content.Responses = new ResponseOption[responses.Length];
                for (int i = 0; i < responses.Length; i++)
                    content.Responses[i] = new ResponseOption(responses[i].Label, responses[i].Description);

                content.OnRespond = index =>
                {
                    _eventDispatcher.RespondToEvent(_state, index);
                    AdvanceQueue();
                };
            }

            Enqueue(content);
        }

        void Enqueue(DialogContent content)
        {
            if (_current != null)
            {
                _queue.Enqueue(content);
                return;
            }

            Show(content);
        }

        void Show(DialogContent content)
        {
            _current = content;
            _wasPaused = _clock.IsPaused;
            _clock.IsPaused = true;
            _backButtonManager?.PushHandler(this);

            EnsureDialog();
            _dialog.Show(content);
        }

        void AdvanceQueue()
        {
            _backButtonManager?.PopHandler(this);
            _dialog?.Hide();
            _current = null;

            if (!_wasPaused) _clock.IsPaused = false;

            if (_queue.Count > 0)
                Show(_queue.Dequeue());
        }

        void EnsureDialog()
        {
            if (_dialog != null) return;
            _dialog = UISystem.GetOrOpen<GUIEventDialog>(UILayer.Popup);
        }

        // ── IBackButtonHandler ────────────────────────────────────────

        public void OnBackButtonPressed()
        {
            if (_current == null) return;

            if (_current.Responses == null || _current.Responses.Length == 0)
            {
                _current.OnDismiss?.Invoke();
            }
            else if (_current.Responses.Length == 1)
            {
                _current.OnRespond?.Invoke(0);
            }
            // Multiple responses: ignore back button
        }

        // ── IDisposable ──────────────────────────────────────────────

        public void Dispose()
        {
            if (_eventDispatcher != null) _eventDispatcher.EventTriggered -= OnEventTriggered;
        }
    }
}
