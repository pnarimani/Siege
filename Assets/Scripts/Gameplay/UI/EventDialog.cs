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
            if (_document == null) _document = gameObject.AddComponent<UIDocument>();

            _root = new VisualElement();
            _root.style.position = Position.Absolute;
            _root.style.left = 0;
            _root.style.right = 0;
            _root.style.top = 0;
            _root.style.bottom = 0;
            _root.style.backgroundColor = new Color(0f, 0f, 0f, 0.85f);
            _root.style.alignItems = Align.Center;
            _root.style.justifyContent = Justify.Center;
            _root.style.display = DisplayStyle.None;
            _root.AddToClassList("event-dialog");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.12f, 0.12f, 0.14f, 1f);
            panel.style.paddingTop = 24;
            panel.style.paddingBottom = 24;
            panel.style.paddingLeft = 32;
            panel.style.paddingRight = 32;
            panel.style.maxWidth = 600;
            panel.style.width = new Length(80, LengthUnit.Percent);
            panel.AddToClassList("event-dialog__panel");

            var closeBtn = new Button { text = "X" };
            closeBtn.style.position = Position.Absolute;
            closeBtn.style.right = 8;
            closeBtn.style.top = 8;
            closeBtn.AddToClassList("event-dialog__close");
            closeBtn.clicked += Dismiss;
            panel.Add(closeBtn);

            _title = new Label();
            _title.style.fontSize = 24;
            _title.style.color = Color.white;
            _title.style.marginBottom = 12;
            _title.AddToClassList("event-dialog__title");
            panel.Add(_title);

            _description = new Label();
            _description.style.fontSize = 14;
            _description.style.color = new Color(0.8f, 0.8f, 0.8f);
            _description.style.marginBottom = 8;
            _description.style.whiteSpace = WhiteSpace.Normal;
            _description.AddToClassList("event-dialog__description");
            panel.Add(_description);

            _narrative = new Label();
            _narrative.style.fontSize = 13;
            _narrative.style.color = new Color(0.7f, 0.7f, 0.6f);
            _narrative.style.marginBottom = 16;
            _narrative.style.whiteSpace = WhiteSpace.Normal;
            _narrative.style.unityFontStyleAndWeight = FontStyle.Italic;
            _narrative.AddToClassList("event-dialog__narrative");
            panel.Add(_narrative);

            _responseContainer = new VisualElement();
            _responseContainer.AddToClassList("event-dialog__responses");
            panel.Add(_responseContainer);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _eventManager = Resolver.Resolve<EventManager>();
            _eventManager.EventTriggered += OnEventTriggered;

            if (_document.rootVisualElement != null && !_document.rootVisualElement.Contains(_root))
                _document.rootVisualElement.Add(_root);
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
                    btn.style.marginTop = 4;
                    btn.style.paddingTop = 8;
                    btn.style.paddingBottom = 8;
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
                btn.style.marginTop = 8;
                btn.style.paddingTop = 8;
                btn.style.paddingBottom = 8;
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
