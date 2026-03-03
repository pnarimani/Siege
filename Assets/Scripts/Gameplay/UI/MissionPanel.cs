using AutofacUnity;
using Siege.Gameplay.Missions;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class MissionPanel : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        ScrollView _availableScroll;
        ScrollView _activeScroll;

        GameState _state;
        GameClock _clock;
        MissionManager _missionManager;

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
            _root.AddToClassList("mission-panel");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.12f, 0.12f, 0.14f, 1f);
            panel.style.paddingTop = 20;
            panel.style.paddingBottom = 20;
            panel.style.paddingLeft = 24;
            panel.style.paddingRight = 24;
            panel.style.maxWidth = 700;
            panel.style.width = new Length(85, LengthUnit.Percent);
            panel.style.maxHeight = new Length(85, LengthUnit.Percent);
            panel.AddToClassList("mission-panel__panel");

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.justifyContent = Justify.SpaceBetween;
            header.style.marginBottom = 12;

            var titleLabel = new Label("Missions");
            titleLabel.style.fontSize = 22;
            titleLabel.style.color = Color.white;
            titleLabel.AddToClassList("mission-panel__title");
            header.Add(titleLabel);

            var closeBtn = new Button { text = "X" };
            closeBtn.AddToClassList("mission-panel__close");
            closeBtn.clicked += Hide;
            header.Add(closeBtn);

            panel.Add(header);

            // Available Missions section
            var availableHeader = new Label("Available Missions");
            availableHeader.style.fontSize = 16;
            availableHeader.style.color = new Color(0.9f, 0.85f, 0.7f);
            availableHeader.style.marginBottom = 8;
            availableHeader.AddToClassList("mission-panel__section-header");
            panel.Add(availableHeader);

            _availableScroll = new ScrollView(ScrollViewMode.Vertical);
            _availableScroll.style.flexGrow = 1;
            _availableScroll.style.minHeight = 100;
            _availableScroll.style.marginBottom = 16;
            _availableScroll.AddToClassList("mission-panel__available-scroll");
            panel.Add(_availableScroll);

            // Active Missions section
            var activeHeader = new Label("Active Missions");
            activeHeader.style.fontSize = 16;
            activeHeader.style.color = new Color(0.7f, 0.85f, 0.9f);
            activeHeader.style.marginBottom = 8;
            activeHeader.AddToClassList("mission-panel__section-header");
            panel.Add(activeHeader);

            _activeScroll = new ScrollView(ScrollViewMode.Vertical);
            _activeScroll.style.flexGrow = 1;
            _activeScroll.style.minHeight = 60;
            _activeScroll.AddToClassList("mission-panel__active-scroll");
            panel.Add(_activeScroll);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _missionManager = Resolver.Resolve<MissionManager>();

            if (_document.rootVisualElement != null && !_document.rootVisualElement.Contains(_root))
                _document.rootVisualElement.Add(_root);
        }

        void Update()
        {
            if (_state == null || _missionManager == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            RebuildAvailable();
            RebuildActive();
        }

        void RebuildAvailable()
        {
            _availableScroll.Clear();
            bool isDay = _clock != null && _clock.IsDay;

            foreach (var kvp in _missionManager.AllMissions)
            {
                var mission = kvp.Value;
                if (mission.IsActive) continue;

                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.justifyContent = Justify.SpaceBetween;
                row.style.alignItems = Align.Center;
                row.style.paddingTop = 6;
                row.style.paddingBottom = 6;
                row.style.borderBottomWidth = 1;
                row.style.borderBottomColor = new Color(0.3f, 0.3f, 0.3f);
                row.AddToClassList("mission-panel__row");

                var info = new VisualElement();
                info.style.flexGrow = 1;
                info.style.flexShrink = 1;

                var nameLabel = new Label(mission.Name);
                nameLabel.style.fontSize = 14;
                nameLabel.style.color = Color.white;
                nameLabel.AddToClassList("mission-panel__mission-name");
                info.Add(nameLabel);

                var descLabel = new Label(mission.Description);
                descLabel.style.fontSize = 11;
                descLabel.style.color = new Color(0.6f, 0.6f, 0.6f);
                descLabel.style.whiteSpace = WhiteSpace.Normal;
                descLabel.AddToClassList("mission-panel__mission-desc");
                info.Add(descLabel);

                string costText = $"Duration: {mission.DurationDays}d | Workers: {mission.WorkerCost}";
                if (mission.GuardCost > 0) costText += $" | Guards: {mission.GuardCost}";
                var costLabel = new Label(costText);
                costLabel.style.fontSize = 11;
                costLabel.style.color = new Color(0.7f, 0.7f, 0.5f);
                costLabel.AddToClassList("mission-panel__cost");
                info.Add(costLabel);

                row.Add(info);

                bool canLaunch = _missionManager.CanLaunch(mission.Id, _state) && !isDay;
                var launchBtn = new Button { text = "Launch" };
                launchBtn.style.marginLeft = 8;
                launchBtn.SetEnabled(canLaunch);
                launchBtn.AddToClassList("mission-panel__launch-btn");
                if (!canLaunch) launchBtn.AddToClassList("mission-panel__launch-btn--disabled");

                if (isDay)
                    launchBtn.tooltip = "Missions can only launch at night";

                string missionId = mission.Id;
                launchBtn.clicked += () => _missionManager.Launch(missionId, _state);
                row.Add(launchBtn);

                _availableScroll.Add(row);
            }
        }

        void RebuildActive()
        {
            _activeScroll.Clear();

            foreach (var mission in _missionManager.ActiveMissions)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.justifyContent = Justify.SpaceBetween;
                row.style.alignItems = Align.Center;
                row.style.paddingTop = 6;
                row.style.paddingBottom = 6;
                row.style.borderBottomWidth = 1;
                row.style.borderBottomColor = new Color(0.25f, 0.25f, 0.3f);
                row.AddToClassList("mission-panel__active-row");

                var nameLabel = new Label(mission.Name);
                nameLabel.style.fontSize = 14;
                nameLabel.style.color = new Color(0.7f, 0.85f, 1f);
                nameLabel.style.flexGrow = 1;
                nameLabel.AddToClassList("mission-panel__active-name");
                row.Add(nameLabel);

                float progress = mission.DurationDays > 0
                    ? 1f - (float)mission.DaysRemaining / mission.DurationDays
                    : 1f;

                var progressBar = new VisualElement();
                progressBar.style.width = 120;
                progressBar.style.height = 12;
                progressBar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
                progressBar.AddToClassList("mission-panel__progress-bg");

                var progressFill = new VisualElement();
                progressFill.style.height = new Length(100, LengthUnit.Percent);
                progressFill.style.width = new Length(progress * 100, LengthUnit.Percent);
                progressFill.style.backgroundColor = new Color(0.3f, 0.6f, 0.9f);
                progressFill.AddToClassList("mission-panel__progress-fill");
                progressBar.Add(progressFill);
                row.Add(progressBar);

                var daysLabel = new Label($"{mission.DaysRemaining}d left");
                daysLabel.style.fontSize = 11;
                daysLabel.style.color = new Color(0.6f, 0.6f, 0.6f);
                daysLabel.style.marginLeft = 8;
                daysLabel.style.width = 50;
                daysLabel.AddToClassList("mission-panel__days-remaining");
                row.Add(daysLabel);

                _activeScroll.Add(row);
            }

            if (_missionManager.ActiveMissions.Count == 0)
            {
                var emptyLabel = new Label("No active missions");
                emptyLabel.style.fontSize = 12;
                emptyLabel.style.color = new Color(0.5f, 0.5f, 0.5f);
                emptyLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                _activeScroll.Add(emptyLabel);
            }
        }

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
