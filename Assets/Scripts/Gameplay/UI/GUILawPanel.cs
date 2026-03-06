using System;
using AutofacUnity;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Orders;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUILawPanel : MonoBehaviour, IBackButtonHandler
    {
        [SerializeField] VisualTreeAsset _rowTemplate;

        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        LawDispatcher _lawDispatcher;
        OrderDispatcher _orderDispatcher;
        GameClock _clock;
        BackButtonManager _backButtonManager;
        bool _dirty = true;

        Action<string> _onLawEnacted;
        Action<string> _onOrderExecuted;
        Action<int> _onDayStarted;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _scrollView = root.Q<ScrollView>("ScrollView");
            root.Q<SiegeButton>("CloseBtn").Clicked += OnBackButtonPressed;
            _backButtonManager = Resolver.Resolve<BackButtonManager>();
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _lawDispatcher = Resolver.Resolve<LawDispatcher>();
            _orderDispatcher = Resolver.Resolve<OrderDispatcher>();
            _clock = Resolver.Resolve<GameClock>();
            _lawDispatcher.LawEnacted += _onLawEnacted = _ => OnBackButtonPressed();
            _orderDispatcher.OrderExecuted += _onOrderExecuted = _ => _dirty = true;
            _clock.DayStarted += _onDayStarted = _ => _dirty = true;
        }

        void Update()
        {
            if (_state == null || _lawDispatcher == null) return;
            if (_root.style.display == DisplayStyle.None) return;
            if (!_dirty) return;
            _dirty = false;

            _scrollView.Clear();

            foreach (var law in _lawDispatcher.AllLaws)
            {
                bool enacted = _lawDispatcher.IsEnacted(law.Id);
                if (!enacted && !_lawDispatcher.CanEnact(law.Id)) continue;

                var row = _rowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = law.Name;
                row.Q<Label>("DescLabel").text = law.Description;

                if (enacted)
                {
                    row.Q("EnactedBadge").style.display = DisplayStyle.Flex;
                    row.Q<SiegeButton>("EnactBtn").style.display = DisplayStyle.None;
                }
                else
                {
                    bool canEnact = _lawDispatcher.CanEnact(law.Id) && !_state.ActionUsedToday;
                    var enactBtn = row.Q<SiegeButton>("EnactBtn");
                    enactBtn.SetEnabled(canEnact);
                    if (!canEnact)
                    {
                        enactBtn.AddToClassList("law-panel__enact-btn--disabled");
                        if (_state.ActionUsedToday)
                            enactBtn.tooltip = "Action already used today";
                    }
                    string lawId = law.Id;
                    enactBtn.Clicked += () => { _lawDispatcher.TryEnact(lawId); _dirty = true; };
                }

                _scrollView.Add(row);
            }
        }

        // ── IBackButtonHandler ────────────────────────────────────────

        public void OnBackButtonPressed() { Hide(); UnityEngine.Object.Destroy(gameObject); }

        void OnDestroy()
        {
            if (_lawDispatcher != null) _lawDispatcher.LawEnacted -= _onLawEnacted;
            if (_orderDispatcher != null) _orderDispatcher.OrderExecuted -= _onOrderExecuted;
            if (_clock != null) _clock.DayStarted -= _onDayStarted;
        }

        // ─────────────────────────────────────────────────────────────

        public bool IsShown => _root.style.display == DisplayStyle.Flex;

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
            _dirty = true;
            _backButtonManager?.PushHandler(this);
        }

        public void Hide()
        {
            _backButtonManager?.PopHandler(this);
            _root.style.display = DisplayStyle.None;
        }
    }
}
