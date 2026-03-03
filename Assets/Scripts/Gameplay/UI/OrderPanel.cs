using AutofacUnity;
using Siege.Gameplay.Orders;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class OrderPanel : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        OrderManager _orderManager;

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
            _root.AddToClassList("order-panel");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.12f, 0.12f, 0.14f, 1f);
            panel.style.paddingTop = 20;
            panel.style.paddingBottom = 20;
            panel.style.paddingLeft = 24;
            panel.style.paddingRight = 24;
            panel.style.maxWidth = 650;
            panel.style.width = new Length(85, LengthUnit.Percent);
            panel.style.maxHeight = new Length(80, LengthUnit.Percent);
            panel.AddToClassList("order-panel__panel");

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.justifyContent = Justify.SpaceBetween;
            header.style.marginBottom = 12;

            var titleLabel = new Label("Orders");
            titleLabel.style.fontSize = 22;
            titleLabel.style.color = Color.white;
            titleLabel.AddToClassList("order-panel__title");
            header.Add(titleLabel);

            var closeBtn = new Button { text = "X" };
            closeBtn.AddToClassList("order-panel__close");
            closeBtn.clicked += Hide;
            header.Add(closeBtn);

            panel.Add(header);

            _scrollView = new ScrollView(ScrollViewMode.Vertical);
            _scrollView.style.flexGrow = 1;
            _scrollView.AddToClassList("order-panel__scroll");
            panel.Add(_scrollView);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _orderManager = Resolver.Resolve<OrderManager>();

            if (_document.rootVisualElement != null && !_document.rootVisualElement.Contains(_root))
                _document.rootVisualElement.Add(_root);
        }

        void Update()
        {
            if (_state == null || _orderManager == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            _scrollView.Clear();

            foreach (var order in _orderManager.AllOrders)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.justifyContent = Justify.SpaceBetween;
                row.style.alignItems = Align.Center;
                row.style.paddingTop = 8;
                row.style.paddingBottom = 8;
                row.style.borderBottomWidth = 1;
                row.style.borderBottomColor = new Color(0.3f, 0.3f, 0.3f);
                row.AddToClassList("order-panel__row");

                var info = new VisualElement();
                info.style.flexGrow = 1;
                info.style.flexShrink = 1;

                var nameRow = new VisualElement();
                nameRow.style.flexDirection = FlexDirection.Row;
                nameRow.style.alignItems = Align.Center;

                var nameLabel = new Label(order.Name);
                nameLabel.style.fontSize = 15;
                nameLabel.style.color = Color.white;
                nameLabel.AddToClassList("order-panel__order-name");
                nameRow.Add(nameLabel);

                if (order.IsToggle && order.IsActive)
                {
                    var activeBadge = new Label("ACTIVE");
                    activeBadge.style.color = new Color(0.3f, 0.7f, 1f);
                    activeBadge.style.fontSize = 11;
                    activeBadge.style.marginLeft = 8;
                    activeBadge.AddToClassList("order-panel__active-badge");
                    nameRow.Add(activeBadge);
                }

                info.Add(nameRow);

                var descLabel = new Label(order.Description);
                descLabel.style.fontSize = 12;
                descLabel.style.color = new Color(0.6f, 0.6f, 0.6f);
                descLabel.style.whiteSpace = WhiteSpace.Normal;
                descLabel.AddToClassList("order-panel__order-desc");
                info.Add(descLabel);

                int cooldown = _orderManager.GetCooldownRemaining(order.Id);
                if (cooldown > 0)
                {
                    var cdLabel = new Label($"Cooldown: {cooldown} day{(cooldown > 1 ? "s" : "")}");
                    cdLabel.style.fontSize = 11;
                    cdLabel.style.color = new Color(0.9f, 0.6f, 0.3f);
                    cdLabel.AddToClassList("order-panel__cooldown");
                    info.Add(cdLabel);
                }

                row.Add(info);

                bool canIssue = order.CanIssue(_state) && cooldown <= 0;

                if (order.IsToggle && order.IsActive)
                {
                    var deactivateBtn = new Button { text = "Deactivate" };
                    deactivateBtn.style.marginLeft = 8;
                    deactivateBtn.SetEnabled(order.CanDeactivate);
                    deactivateBtn.AddToClassList("order-panel__deactivate-btn");
                    string orderId = order.Id;
                    deactivateBtn.clicked += () => _orderManager.TryDeactivate(orderId);
                    row.Add(deactivateBtn);
                }
                else
                {
                    string btnText = order.IsToggle ? "Activate" : "Execute";
                    var execBtn = new Button { text = btnText };
                    execBtn.style.marginLeft = 8;
                    execBtn.SetEnabled(canIssue);
                    execBtn.AddToClassList("order-panel__execute-btn");
                    if (!canIssue) execBtn.AddToClassList("order-panel__execute-btn--disabled");

                    string orderId = order.Id;
                    execBtn.clicked += () => _orderManager.TryExecute(orderId);
                    row.Add(execBtn);
                }

                _scrollView.Add(row);
            }
        }

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
