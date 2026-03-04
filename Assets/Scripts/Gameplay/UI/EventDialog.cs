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
        VisualElement _responseContainer;

        GameState _state;
        GameClock _clock;
        EventManager _eventManager;
        bool _wasPaused;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _title = root.Q<Label>("Title");
            _description = root.Q<Label>("Description");
            _narrative = root.Q<Label>("Narrative");
            _responseContainer = root.Q("ResponseContainer");
            root.Q<Button>("CloseBtn").clicked += Dismiss;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _eventManager = Resolver.Resolve<EventManager>();
            _eventManager.EventTriggered += OnEventTriggered;
        }

        void OnDestroy()
        {
            if (_eventManager != null) _eventManager.EventTriggered -= OnEventTriggered;
        }

        void OnEventTriggered(GameEvent evt)
        {
            _title.text = evt.Name;
            _description.text = evt.Description;
            _narrative.text = evt.GetNarrativeText(_state);

            _responseContainer.Clear();

            if (evt.IsRespondable)
            {
                var responses = evt.GetResponses(_state);
                for (int i = 0; i < responses.Length; i++)
                {
                    int index = i;
                    var response = responses[i];
                    var btn = new Button { text = response.Label };
                    btn.AddToClassList("event-dialog__response-btn");
                    if (!string.IsNullOrEmpty(response.Description))
                        btn.tooltip = response.Description;
                    btn.clicked += () => Respond(index);
                    _responseContainer.Add(btn);
                }
            }
            else
            {
                var btn = new Button { text = "Continue" };
                btn.AddToClassList("event-dialog__continue-btn");
                btn.clicked += Dismiss;
                _responseContainer.Add(btn);
            }

            Show();
            _wasPaused = _clock.IsPaused;
            _clock.IsPaused = true;
        }

        void Respond(int index)
        {
            _eventManager.RespondToEvent(_state, index);
            Hide();
            if (!_wasPaused) _clock.IsPaused = false;
        }

        void Dismiss()
        {
            _eventManager.DismissEvent();
            Hide();
            if (!_wasPaused) _clock.IsPaused = false;
        }

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
