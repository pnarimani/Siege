using AutofacUnity;
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
        GameClock _clock;
        BackButtonManager _backButtonManager;
        bool _dirty = true;

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
            _clock = Resolver.Resolve<GameClock>();
            _orderDispatcher.OrderExecuted += _ => _dirty = true;
            _clock.DayStarted += _ => _dirty = true;
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
                bool isActive = _orderDispatcher.IsActive(order.Id);
                bool unavailable = !isActive
                    && _orderDispatcher.GetCooldownRemaining(order.Id) == 0
                    && !_orderDispatcher.CanIssue(order.Id);
                if (unavailable) continue;

                var row = _rowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = order.Name;
                row.Q<Label>("DescLabel").text = order.Description;

                var activeBadge = row.Q("ActiveBadge");
                activeBadge.style.display = (order.IsToggle && isActive) ? DisplayStyle.Flex : DisplayStyle.None;

                int cooldown = _orderDispatcher.GetCooldownRemaining(order.Id);
                var cooldownLabel = row.Q<Label>("CooldownLabel");
                if (cooldown > 0)
                {
                    cooldownLabel.text = $"Cooldown: {cooldown} day{(cooldown > 1 ? "s" : "")}";
                    cooldownLabel.style.display = DisplayStyle.Flex;
                }

                var executeBtn = row.Q<SiegeButton>("ExecuteBtn");
                var deactivateBtn = row.Q<SiegeButton>("DeactivateBtn");

                if (order.IsToggle && isActive)
                {
                    executeBtn.style.display = DisplayStyle.None;
                    deactivateBtn.style.display = DisplayStyle.Flex;
                    deactivateBtn.SetEnabled(order.CanDeactivate);
                    string orderId = order.Id;
                    deactivateBtn.Clicked += () => { _orderDispatcher.TryDeactivate(orderId); _dirty = true; };
                }
                else
                {
                    executeBtn.Text = order.IsToggle ? "Activate" : "Execute";
                    bool canIssue = _orderDispatcher.CanIssue(order.Id) && cooldown <= 0;
                    executeBtn.SetEnabled(canIssue);
                    if (!canIssue) executeBtn.AddToClassList("order-panel__execute-btn--disabled");
                    string orderId = order.Id;
                    executeBtn.Clicked += () => { _orderDispatcher.TryExecute(orderId); _dirty = true; };
                }

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

        public void OnBackButtonPressed() { Hide(); Object.Destroy(gameObject); }
    }
}
