using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Events;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class NarrativeLog : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        EventManager _eventManager;
        GameState _state;
        readonly List<string> _entries = new();

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
            _root.style.backgroundColor = new Color(0f, 0f, 0f, 0.7f);
            _root.style.alignItems = Align.Center;
            _root.style.justifyContent = Justify.Center;
            _root.style.display = DisplayStyle.None;
            _root.AddToClassList("narrative-log");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.1f, 0.1f, 0.12f, 1f);
            panel.style.paddingTop = 20;
            panel.style.paddingBottom = 20;
            panel.style.paddingLeft = 24;
            panel.style.paddingRight = 24;
            panel.style.maxWidth = 600;
            panel.style.width = new Length(80, LengthUnit.Percent);
            panel.style.maxHeight = new Length(80, LengthUnit.Percent);
            panel.AddToClassList("narrative-log__panel");

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.justifyContent = Justify.SpaceBetween;
            header.style.marginBottom = 12;

            var titleLabel = new Label("Narrative Log");
            titleLabel.style.fontSize = 22;
            titleLabel.style.color = Color.white;
            titleLabel.AddToClassList("narrative-log__title");
            header.Add(titleLabel);

            var closeBtn = new Button { text = "X" };
            closeBtn.AddToClassList("narrative-log__close");
            closeBtn.clicked += Hide;
            header.Add(closeBtn);

            panel.Add(header);

            _scrollView = new ScrollView(ScrollViewMode.Vertical);
            _scrollView.style.flexGrow = 1;
            _scrollView.AddToClassList("narrative-log__scroll");
            panel.Add(_scrollView);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
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
            string narrative = evt.GetNarrativeText(_state);
            if (!string.IsNullOrEmpty(narrative))
                AddEntry(narrative);
        }

        public void AddEntry(string text)
        {
            _entries.Add(text);

            var label = new Label(text);
            label.style.fontSize = 13;
            label.style.color = new Color(0.75f, 0.75f, 0.7f);
            label.style.marginBottom = 6;
            label.style.paddingBottom = 6;
            label.style.borderBottomWidth = 1;
            label.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f);
            label.style.whiteSpace = WhiteSpace.Normal;
            label.AddToClassList("narrative-log__entry");
            _scrollView.Add(label);

            // Auto-scroll to bottom
            _scrollView.schedule.Execute(() =>
                _scrollView.scrollOffset = new Vector2(0, float.MaxValue));
        }

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
