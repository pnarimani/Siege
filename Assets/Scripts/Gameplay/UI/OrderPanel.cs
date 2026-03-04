using AutofacUnity;
using Siege.Gameplay.Orders;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class OrderPanel : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset _rowTemplate;

        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        OrderManager _orderManager;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _scrollView = root.Q<ScrollView>("ScrollView");
            root.Q<Button>("CloseBtn").clicked += Hide;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _orderManager = Resolver.Resolve<OrderManager>();
        }

        void Update()
        {
            if (_state == null || _orderManager == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            _scrollView.Clear();

            foreach (var order in _orderManager.AllOrders)
            {
                var row = _rowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = order.Name;
                row.Q<Label>("DescLabel").text = order.Description;

                var activeBadge = row.Q("ActiveBadge");
                activeBadge.style.display = (order.IsToggle && order.IsActive) ? DisplayStyle.Flex : DisplayStyle.None;

                int cooldown = _orderManager.GetCooldownRemaining(order.Id);
                var cooldownLabel = row.Q<Label>("CooldownLabel");
                if (cooldown > 0)
                {
                    cooldownLabel.text = $"Cooldown: {cooldown} day{(cooldown > 1 ? "s" : "")}";
                    cooldownLabel.style.display = DisplayStyle.Flex;
                }

                var executeBtn = row.Q<Button>("ExecuteBtn");
                var deactivateBtn = row.Q<Button>("DeactivateBtn");

                if (order.IsToggle && order.IsActive)
                {
                    executeBtn.style.display = DisplayStyle.None;
                    deactivateBtn.style.display = DisplayStyle.Flex;
                    deactivateBtn.SetEnabled(order.CanDeactivate);
                    string orderId = order.Id;
                    deactivateBtn.clicked += () => _orderManager.TryDeactivate(orderId);
                }
                else
                {
                    executeBtn.text = order.IsToggle ? "Activate" : "Execute";
                    bool canIssue = order.CanIssue(_state) && cooldown <= 0;
                    executeBtn.SetEnabled(canIssue);
                    if (!canIssue) executeBtn.AddToClassList("order-panel__execute-btn--disabled");
                    string orderId = order.Id;
                    executeBtn.clicked += () => _orderManager.TryExecute(orderId);
                }

                _scrollView.Add(row);
            }
        }

        public bool IsShown => _root.style.display == DisplayStyle.Flex;
        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
