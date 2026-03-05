using AutofacUnity;
using Siege.Gameplay.Missions;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIMissionPanel : MonoBehaviour, IBackButtonHandler
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
        BackButtonManager _backButtonManager;
        bool _dirty = true;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _availableScroll = root.Q<ScrollView>("AvailableScroll");
            _activeScroll = root.Q<ScrollView>("ActiveScroll");
            root.Q<SiegeButton>("CloseBtn").Clicked += OnBackButtonPressed;
            _backButtonManager = Resolver.Resolve<BackButtonManager>();
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();
            _missionDispatcher = Resolver.Resolve<MissionDispatcher>();
            _missionDispatcher.MissionLaunched += _ => OnBackButtonPressed();
            _missionDispatcher.MissionCompleted += (_, _) => _dirty = true;
            _clock.DayStarted += _ => _dirty = true;
            _clock.NightStarted += _ => _dirty = true;
        }

        void Update()
        {
            if (_state == null || _missionDispatcher == null) return;
            if (_root.style.display == DisplayStyle.None) return;
            if (!_dirty) return;
            _dirty = false;

            RebuildAvailable();
            RebuildActive();
        }

        void RebuildAvailable()
        {
            _availableScroll.Clear();
            bool isDay = _clock != null && _clock.IsDay;

            foreach (var mission in _missionDispatcher.AllMissions)
            {
                if (_missionDispatcher.IsActive(mission.Id)) continue;
                if (!_missionDispatcher.CanLaunch(mission.Id, _state)) continue;

                var row = _availableRowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = mission.Name;
                row.Q<Label>("DescLabel").text = mission.Description;

                bool canLaunch = _missionDispatcher.CanLaunch(mission.Id, _state) && !isDay && !_state.MissionLaunchedThisNight;
                var launchBtn = row.Q<SiegeButton>("LaunchBtn");
                launchBtn.SetEnabled(canLaunch);
                if (!canLaunch) launchBtn.AddToClassList("mission-panel__launch-btn--disabled");
                if (isDay) launchBtn.tooltip = "Missions can only launch at night";
                else if (_state.MissionLaunchedThisNight) launchBtn.tooltip = "Only one mission can launch per night";

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
                float progress = mission.Progress;

                var row = _activeRowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = mission.Name;
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
