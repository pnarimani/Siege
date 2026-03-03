using AutofacUnity;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class LawPanel : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        LawManager _lawManager;

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
            _root.AddToClassList("law-panel");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.12f, 0.12f, 0.14f, 1f);
            panel.style.paddingTop = 20;
            panel.style.paddingBottom = 20;
            panel.style.paddingLeft = 24;
            panel.style.paddingRight = 24;
            panel.style.maxWidth = 650;
            panel.style.width = new Length(85, LengthUnit.Percent);
            panel.style.maxHeight = new Length(80, LengthUnit.Percent);
            panel.AddToClassList("law-panel__panel");

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.justifyContent = Justify.SpaceBetween;
            header.style.marginBottom = 12;

            var titleLabel = new Label("Laws");
            titleLabel.style.fontSize = 22;
            titleLabel.style.color = Color.white;
            titleLabel.AddToClassList("law-panel__title");
            header.Add(titleLabel);

            var closeBtn = new Button { text = "X" };
            closeBtn.AddToClassList("law-panel__close");
            closeBtn.clicked += Hide;
            header.Add(closeBtn);

            panel.Add(header);

            _scrollView = new ScrollView(ScrollViewMode.Vertical);
            _scrollView.style.flexGrow = 1;
            _scrollView.AddToClassList("law-panel__scroll");
            panel.Add(_scrollView);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _lawManager = Resolver.Resolve<LawManager>();

            if (_document.rootVisualElement != null && !_document.rootVisualElement.Contains(_root))
                _document.rootVisualElement.Add(_root);
        }

        void Update()
        {
            if (_state == null || _lawManager == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            _scrollView.Clear();

            foreach (var law in _lawManager.AllLaws)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.justifyContent = Justify.SpaceBetween;
                row.style.alignItems = Align.Center;
                row.style.paddingTop = 8;
                row.style.paddingBottom = 8;
                row.style.borderBottomWidth = 1;
                row.style.borderBottomColor = new Color(0.3f, 0.3f, 0.3f);
                row.AddToClassList("law-panel__row");

                var info = new VisualElement();
                info.style.flexGrow = 1;
                info.style.flexShrink = 1;

                var nameLabel = new Label(law.Name);
                nameLabel.style.fontSize = 15;
                nameLabel.style.color = Color.white;
                nameLabel.AddToClassList("law-panel__law-name");
                info.Add(nameLabel);

                var descLabel = new Label(law.Description);
                descLabel.style.fontSize = 12;
                descLabel.style.color = new Color(0.6f, 0.6f, 0.6f);
                descLabel.style.whiteSpace = WhiteSpace.Normal;
                descLabel.AddToClassList("law-panel__law-desc");
                info.Add(descLabel);

                row.Add(info);

                if (law.IsEnacted)
                {
                    var badge = new Label("ENACTED");
                    badge.style.color = new Color(0.4f, 0.8f, 0.4f);
                    badge.style.fontSize = 12;
                    badge.style.marginLeft = 8;
                    badge.AddToClassList("law-panel__enacted-badge");
                    row.Add(badge);
                }
                else
                {
                    bool canEnact = law.CanEnact(_state);
                    var enactBtn = new Button { text = "Enact" };
                    enactBtn.style.marginLeft = 8;
                    enactBtn.SetEnabled(canEnact);
                    enactBtn.AddToClassList("law-panel__enact-btn");
                    if (!canEnact) enactBtn.AddToClassList("law-panel__enact-btn--disabled");

                    string lawId = law.Id;
                    enactBtn.clicked += () => _lawManager.TryEnact(lawId);
                    row.Add(enactBtn);
                }

                _scrollView.Add(row);
            }
        }

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
