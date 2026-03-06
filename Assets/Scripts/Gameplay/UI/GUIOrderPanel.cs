using System;
using AutofacUnity;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Orders;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIOrderPanel : MonoBehaviour, IBackButtonHandler
    {
        [SerializeField] VisualTreeAsset _rowTemplate;

        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        OrderDispatcher _orderDispatcher;
        LawDispatcher _lawDispatcher;
        GameClock _clock;
        BackButtonManager _backButtonManager;
        bool _dirty = true;

        Action<string> _onOrderExecuted;
        Action<string> _onLawEnacted;
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
            _orderDispatcher = Resolver.Resolve<OrderDispatcher>();
            _lawDispatcher = Resolver.Resolve<LawDispatcher>();
            _clock = Resolver.Resolve<GameClock>();
            _orderDispatcher.OrderExecuted += _onOrderExecuted = _ => OnBackButtonPressed();
            _lawDispatcher.LawEnacted += _onLawEnacted = _ => _dirty = true;
            _clock.DayStarted += _onDayStarted = _ => _dirty = true;
        }

        void Update()
        {
            if (_state == null || _orderDispatcher == null) return;
            if (_root.style.display == DisplayStyle.None) return;
            if (!_dirty) return;
            _dirty = false;

            _scrollView.Clear();

            foreach (var order in _orderDispatcher.AllOrders)
            {
                if (_orderDispatcher.GetCooldownRemaining(order.Id) == 0
                    && !_orderDispatcher.CanIssue(order.Id))
                    continue;

                var row = _rowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = order.Name;
                row.Q<Label>("DescLabel").text = order.Description;

                int cooldown = _orderDispatcher.GetCooldownRemaining(order.Id);
                var cooldownLabel = row.Q<Label>("CooldownLabel");
                if (_state.ActionUsedToday && cooldown == 0)
                {
                    cooldownLabel.text = "Action used today";
                    cooldownLabel.style.display = DisplayStyle.Flex;
                }
                else if (cooldown > 0)
                {
                    cooldownLabel.text = $"Cooldown: {cooldown} day{(cooldown > 1 ? "s" : "")}";
                    cooldownLabel.style.display = DisplayStyle.Flex;
                }

                var executeBtn = row.Q<SiegeButton>("ExecuteBtn");
                executeBtn.Text = "Execute";
                bool canIssue = _orderDispatcher.CanIssue(order.Id) && cooldown <= 0 && !_state.ActionUsedToday;
                executeBtn.SetEnabled(canIssue);
                if (!canIssue) executeBtn.AddToClassList("order-panel__execute-btn--disabled");
                string orderId = order.Id;
                executeBtn.Clicked += () => { _orderDispatcher.TryExecute(orderId); _dirty = true; };

                _scrollView.Add(row);
            }
        }

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

        // ── IBackButtonHandler ────────────────────────────────────────

        public void OnBackButtonPressed() { Hide(); UnityEngine.Object.Destroy(gameObject); }

        void OnDestroy()
        {
            if (_orderDispatcher != null) _orderDispatcher.OrderExecuted -= _onOrderExecuted;
            if (_lawDispatcher != null) _lawDispatcher.LawEnacted -= _onLawEnacted;
            if (_clock != null) _clock.DayStarted -= _onDayStarted;
        }
    }
}
