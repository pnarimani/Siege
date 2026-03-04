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
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _scrollView = root.Q<ScrollView>("ScrollView");
            root.Q<SiegeButton>("CloseBtn").Clicked += Hide;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _eventManager = Resolver.Resolve<EventManager>();
            _eventManager.EventTriggered += OnEventTriggered;
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
