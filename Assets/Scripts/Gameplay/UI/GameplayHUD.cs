using AutofacUnity;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GameplayHUD : MonoBehaviour
    {
        [SerializeField] LocalizedString _foodTitle, _foodDesc;
        [SerializeField] LocalizedString _waterTitle, _waterDesc;
        [SerializeField] LocalizedString _fuelTitle, _fuelDesc;
        [SerializeField] LocalizedString _materialsTitle, _materialsDesc;
        [SerializeField] LocalizedString _medsTitle, _medsDesc;
        [SerializeField] LocalizedString _moraleTitle, _moraleDesc;
        [SerializeField] LocalizedString _unrestTitle, _unrestDesc;
        [SerializeField] LocalizedString _sicknessTitle, _sicknessDesc;

        ResourceWidget _food, _water, _fuel, _materials, _meds;
        ProgressBar _morale, _unrest, _sickness;

        SiegeButton _lawsBtn, _ordersBtn, _missionsBtn;
        Label _dayLabel, _phaseLabel;

        LawPanel _lawPanel;
        OrderPanel _orderPanel;
        MissionPanel _missionPanel;

        GameState _state;
        GameClock _clock;
        NotificationPanel _notificationPanel;

        void Awake()
        {
            _food = this.FindElement<ResourceWidget>("Food");
            _water = this.FindElement<ResourceWidget>("Water");
            _fuel = this.FindElement<ResourceWidget>("Fuel");
            _materials = this.FindElement<ResourceWidget>("Materials");
            _meds = this.FindElement<ResourceWidget>("Meds");

            _morale = this.FindElement<ProgressBar>("Morale");
            _unrest = this.FindElement<ProgressBar>("Unrest");
            _sickness = this.FindElement<ProgressBar>("Sickness");

            _morale?.AddToClassList("progress-bar--morale");
            _unrest?.AddToClassList("progress-bar--unrest");
            _sickness?.AddToClassList("progress-bar--sickness");

            _dayLabel = this.FindElement<Label>("DayLabel");
            _phaseLabel = this.FindElement<Label>("PhaseLabel");

            _lawsBtn = this.FindElement<SiegeButton>("LawsBtn");
            _ordersBtn = this.FindElement<SiegeButton>("OrdersBtn");
            _missionsBtn = this.FindElement<SiegeButton>("MissionsBtn");

            if (_lawsBtn != null) _lawsBtn.Clicked += OnLawsClicked;
            if (_ordersBtn != null) _ordersBtn.Clicked += OnOrdersClicked;
            if (_missionsBtn != null) _missionsBtn.Clicked += OnMissionsClicked;

            SetupTooltip(_food, _foodTitle, _foodDesc);
            SetupTooltip(_water, _waterTitle, _waterDesc);
            SetupTooltip(_fuel, _fuelTitle, _fuelDesc);
            SetupTooltip(_materials, _materialsTitle, _materialsDesc);
            SetupTooltip(_meds, _medsTitle, _medsDesc);
            SetupTooltip(_morale, _moraleTitle, _moraleDesc);
            SetupTooltip(_unrest, _unrestTitle, _unrestDesc);
            SetupTooltip(_sickness, _sicknessTitle, _sicknessDesc);
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _clock = Resolver.Resolve<GameClock>();

            var notificationList = this.FindElement<VisualElement>("NotificationList");
            var notificationService = Resolver.Resolve<NotificationService>();
            _notificationPanel = new NotificationPanel(notificationList, notificationService);
        }

        void OnDestroy()
        {
            _notificationPanel?.Dispose();
        }

        void Update()
        {
            if (_state == null) return;
            UpdateResources();
            UpdateStatusBars();
            UpdateActionBar();
        }

        int _prevFood, _prevWater, _prevFuel, _prevMaterials, _prevMeds;

        void UpdateResources()
        {
            UpdateResource(_food, ref _prevFood, (int)_state.Food);
            UpdateResource(_water, ref _prevWater, (int)_state.Water);
            UpdateResource(_fuel, ref _prevFuel, (int)_state.Fuel);
            UpdateResource(_materials, ref _prevMaterials, (int)_state.Materials);
            UpdateResource(_meds, ref _prevMeds, (int)_state.Medicine);
        }

        static void UpdateResource(ResourceWidget widget, ref int prev, int current)
        {
            if (widget == null || prev == current) return;
            prev = current;
            widget.Text = current.ToString();
        }

        void UpdateStatusBars()
        {
            _morale?.Set01((float)(_state.Morale / 100.0));
            _unrest?.Set01((float)(_state.Unrest / 100.0));
            _sickness?.Set01((float)(_state.Sickness / 100.0));
        }

        void UpdateActionBar()
        {
            if (_clock == null) return;

            bool isDay = _clock.IsDay;

            if (_dayLabel != null)
                _dayLabel.text = $"Day {_clock.CurrentDay}";

            if (_phaseLabel != null)
            {
                _phaseLabel.text = isDay ? "DAY" : "NIGHT";
                _phaseLabel.EnableInClassList("gameplay-hud__phase-label--day", isDay);
                _phaseLabel.EnableInClassList("gameplay-hud__phase-label--night", !isDay);
            }

            _lawsBtn?.SetEnabled(isDay);
            _ordersBtn?.SetEnabled(isDay);
            _missionsBtn?.SetEnabled(!isDay);
        }

        void OnLawsClicked()
        {
            bool wasShown = _lawPanel != null;
            CloseAllPanels();
            if (!wasShown) { _lawPanel = UISystem.Open<LawPanel>(UILayer.Window); _lawPanel?.Show(); _lawsBtn?.AddToClassList("hud-btn--active"); }
        }

        void OnOrdersClicked()
        {
            bool wasShown = _orderPanel != null;
            CloseAllPanels();
            if (!wasShown) { _orderPanel = UISystem.Open<OrderPanel>(UILayer.Window); _orderPanel?.Show(); _ordersBtn?.AddToClassList("hud-btn--active"); }
        }

        void OnMissionsClicked()
        {
            bool wasShown = _missionPanel != null;
            CloseAllPanels();
            if (!wasShown) { _missionPanel = UISystem.Open<MissionPanel>(UILayer.Window); _missionPanel?.Show(); _missionsBtn?.AddToClassList("hud-btn--active"); }
        }

        void CloseAllPanels()
        {
            if (_lawPanel != null) { Object.Destroy(_lawPanel.gameObject); _lawPanel = null; }
            _lawsBtn?.RemoveFromClassList("hud-btn--active");
            if (_orderPanel != null) { Object.Destroy(_orderPanel.gameObject); _orderPanel = null; }
            _ordersBtn?.RemoveFromClassList("hud-btn--active");
            if (_missionPanel != null) { Object.Destroy(_missionPanel.gameObject); _missionPanel = null; }
            _missionsBtn?.RemoveFromClassList("hud-btn--active");
        }

        static void SetupTooltip(VisualElement el, LocalizedString title, LocalizedString desc)
        {
            if (el == null) return;
            el.pickingMode = PickingMode.Position;
            el.AddManipulator(new TooltipManipulator(
                title is { IsEmpty: false } ? title.GetLocalizedString() : "",
                desc is { IsEmpty: false } ? desc.GetLocalizedString() : null
            ));
        }
    }
}