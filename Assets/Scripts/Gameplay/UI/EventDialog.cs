using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Events;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class EventDialog : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        Label _title;
        Label _description;
        Label _narrative;
        VisualElement _changesContainer;
        VisualElement _responseContainer;

        GameState _state;
        GameClock _clock;
        EventManager _eventManager;
        bool _wasPaused;

        bool _isShowing;
        readonly Queue<PopupRequest> _popupQueue = new();

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _title = root.Q<Label>("Title");
            _description = root.Q<Label>("Description");
            _narrative = root.Q<Label>("Narrative");
            _changesContainer = root.Q("ChangesContainer");
            _responseContainer = root.Q("ResponseContainer");
            root.Q<SiegeButton>("CloseBtn").Clicked += Dismiss;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _eventManager = Resolver.Resolve<EventManager>();
            _eventManager.EventTriggered += OnEventTriggered;
            Popup.Requested += OnPopupRequested;
        }

        void OnDestroy()
        {
            if (_eventManager != null) _eventManager.EventTriggered -= OnEventTriggered;
            Popup.Requested -= OnPopupRequested;
        }

        void OnEventTriggered(GameEvent evt)
        {
            if (_isShowing)
            {
                // Queue as popup request (no changes section for events)
                _popupQueue.Enqueue(new PopupRequest
                {
                    Title = evt.Name,
                    Narrative = evt.GetNarrativeText(_state),
                    Changes = null
                });
                return;
            }

            ShowEvent(evt);
        }

        void OnPopupRequested(PopupRequest req)
        {
            if (_isShowing)
            {
                _popupQueue.Enqueue(req);
                return;
            }

            ShowPopup(req);
        }

        void ShowEvent(GameEvent evt)
        {
            _title.text = evt.Name;
            _description.text = evt.Description;
            _description.style.display = DisplayStyle.Flex;
            _narrative.text = evt.GetNarrativeText(_state);
            _changesContainer.style.display = DisplayStyle.None;

            _responseContainer.Clear();

            if (evt.IsRespondable)
            {
                var responses = evt.GetResponses(_state);
                for (int i = 0; i < responses.Length; i++)
                {
                    int index = i;
                    var response = responses[i];
                    var btn = new SiegeButton { Text = response.Label };
                    btn.AddToClassList("event-dialog__response-btn");
                    if (!string.IsNullOrEmpty(response.Description))
                        btn.tooltip = response.Description;
                    btn.Clicked += () => RespondToEvent(index);
                    _responseContainer.Add(btn);
                }
            }
            else
            {
                var btn = new SiegeButton { Text = "Continue" };
                btn.AddToClassList("event-dialog__continue-btn");
                btn.Clicked += Dismiss;
                _responseContainer.Add(btn);
            }

            Show();
        }

        void ShowPopup(PopupRequest req)
        {
            _title.text = req.Title;
            _description.style.display = DisplayStyle.None;
            _narrative.text = req.Narrative;

            _changesContainer.Clear();
            if (req.Changes != null && req.Changes.Count > 0)
            {
                foreach (var change in req.Changes)
                {
                    var label = new Label(StateChangeFormatter.Format(change));
                    label.AddToClassList("event-dialog__change-entry");
                    _changesContainer.Add(label);
                }
                _changesContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _changesContainer.style.display = DisplayStyle.None;
            }

            _responseContainer.Clear();
            var okBtn = new SiegeButton { Text = "OK" };
            okBtn.AddToClassList("event-dialog__continue-btn");
            okBtn.Clicked += Dismiss;
            _responseContainer.Add(okBtn);

            Show();
        }

        void RespondToEvent(int index)
        {
            _eventManager.RespondToEvent(_state, index);
            HideAndAdvanceQueue();
        }

        void Dismiss()
        {
            _eventManager.DismissEvent();
            HideAndAdvanceQueue();
        }

        void HideAndAdvanceQueue()
        {
            Hide();
            if (!_wasPaused) _clock.IsPaused = false;

            if (_popupQueue.Count > 0)
            {
                var next = _popupQueue.Dequeue();
                // Enqueued events were stored as PopupRequests (no changes); show them as popups
                ShowPopup(next);
            }
        }

        void Show()
        {
            _root.style.display = DisplayStyle.Flex;
            _isShowing = true;
            _wasPaused = _clock.IsPaused;
            _clock.IsPaused = true;
        }

        void Hide()
        {
            _root.style.display = DisplayStyle.None;
            _isShowing = false;
        }
    }
}
