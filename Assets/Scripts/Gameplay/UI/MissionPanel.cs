using AutofacUnity;
using Siege.Gameplay.Missions;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class MissionPanel : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset _availableRowTemplate;
        [SerializeField] VisualTreeAsset _activeRowTemplate;

        UIDocument _document;
        VisualElement _root;
        ScrollView _availableScroll;
        ScrollView _activeScroll;

        GameState _state;
        GameClock _clock;
        MissionDispatcher _missionDispatcher;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _availableScroll = root.Q<ScrollView>("AvailableScroll");
            _activeScroll = root.Q<ScrollView>("ActiveScroll");
            root.Q<SiegeButton>("CloseBtn").Clicked += Hide;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _missionDispatcher = Resolver.Resolve<MissionDispatcher>();
        }

        void Update()
        {
            if (_state == null || _missionDispatcher == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            RebuildAvailable();
            RebuildActive();
        }

        void RebuildAvailable()
        {
            _availableScroll.Clear();
            bool isDay = _clock != null && _clock.IsDay;

            foreach (var kvp in _missionDispatcher.AllMissions)
            {
                var mission = kvp.Value;
                if (mission.IsActive) continue;

                var row = _availableRowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = mission.Name;
                row.Q<Label>("DescLabel").text = mission.Description;

                string costText = $"Duration: {mission.DurationDays}d | Workers: {mission.WorkerCost}";
                if (mission.GuardCost > 0) costText += $" | Guards: {mission.GuardCost}";
                row.Q<Label>("CostLabel").text = costText;

                bool canLaunch = _missionDispatcher.CanLaunch(mission.Id, _state) && !isDay;
                var launchBtn = row.Q<SiegeButton>("LaunchBtn");
                launchBtn.SetEnabled(canLaunch);
                if (!canLaunch) launchBtn.AddToClassList("mission-panel__launch-btn--disabled");
                if (isDay) launchBtn.tooltip = "Missions can only launch at night";

                string missionId = mission.Id;
                launchBtn.Clicked += () => _missionDispatcher.Launch(missionId, _state);
                _availableScroll.Add(row);
            }
        }

        void RebuildActive()
        {
            _activeScroll.Clear();

            foreach (var mission in _missionDispatcher.ActiveMissions)
            {
                float progress = mission.DurationDays > 0
                    ? 1f - (float)mission.DaysRemaining / mission.DurationDays
                    : 1f;

                var row = _activeRowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = mission.Name;
                row.Q<Label>("DaysLabel").text = $"{mission.DaysRemaining}d left";
                row.Q("ProgressFill").style.width = new Length(progress * 100, LengthUnit.Percent);
                _activeScroll.Add(row);
            }

            if (_missionDispatcher.ActiveMissions.Count == 0)
            {
                var emptyLabel = new Label("No active missions");
                emptyLabel.AddToClassList("mission-panel__empty-label");
                _activeScroll.Add(emptyLabel);
            }
        }

        public bool IsShown => _root.style.display == DisplayStyle.Flex;
        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
