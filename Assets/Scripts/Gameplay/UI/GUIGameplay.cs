using System;
using AutofacUnity;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIGameplay : MonoBehaviour
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
        ProgressBar _dayProgress;

        GUILawPanel _lawPanel;
        GUIOrderPanel _orderPanel;
        GUIMissionPanel _missionPanel;

        GameState _state;
        GameClock _clock;
        ResourceLedger _ledger;
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
            _dayProgress = this.FindElement<ProgressBar>("DayProgress");

            _lawsBtn = this.FindElement<SiegeButton>("LawsBtn");
            _ordersBtn = this.FindElement<SiegeButton>("OrdersBtn");
            _missionsBtn = this.FindElement<SiegeButton>("MissionsBtn");

            if (_lawsBtn != null) _lawsBtn.Clicked += OnLawsClicked;
            if (_ordersBtn != null) _ordersBtn.Clicked += OnOrdersClicked;
            if (_missionsBtn != null) _missionsBtn.Clicked += OnMissionsClicked;

            SetupActionButtonTooltip(_lawsBtn, "Laws",
                () => GetActionButtonDescription(true, "Enact a new law for the city"));
            SetupActionButtonTooltip(_ordersBtn, "Orders",
                () => GetActionButtonDescription(true, "Issue an order to your people"));
            SetupActionButtonTooltip(_missionsBtn, "Missions",
                () => GetMissionsButtonDescription());

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
            _ledger = Resolver.Resolve<ResourceLedger>();

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
            if (_state == null || _ledger == null) return;
            UpdateResources();
            UpdateStatusBars();
            UpdateActionBar();
            CleanupDestroyedPanels();
        }

        int _prevFood, _prevWater, _prevFuel, _prevMaterials, _prevMeds;

        void UpdateResources()
        {
            UpdateResource(_food, ref _prevFood, (int)_ledger.GetTotal(ResourceType.Food));
            UpdateResource(_water, ref _prevWater, (int)_ledger.GetTotal(ResourceType.Water));
            UpdateResource(_fuel, ref _prevFuel, (int)_ledger.GetTotal(ResourceType.Fuel));
            UpdateResource(_materials, ref _prevMaterials, (int)_ledger.GetTotal(ResourceType.Materials));
            UpdateResource(_meds, ref _prevMeds, (int)_ledger.GetTotal(ResourceType.Medicine));
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

            var isDay = _clock.IsDay;

            if (_dayLabel != null)
                _dayLabel.text = $"Day {_clock.CurrentDay}";

            if (_phaseLabel != null)
            {
                _phaseLabel.text = isDay ? "DAY" : "NIGHT";
                _phaseLabel.EnableInClassList("gameplay-hud__phase-label--day", isDay);
                _phaseLabel.EnableInClassList("gameplay-hud__phase-label--night", !isDay);
            }

            if (_dayProgress != null)
            {
                _dayProgress.Set01(_clock.DayProgress);
                _dayProgress.EnableInClassList("gameplay-hud__day-progress--night", !isDay);
            }

            _lawsBtn?.SetEnabled(isDay && !_state.ActionUsedToday);
            _ordersBtn?.SetEnabled(isDay && !_state.ActionUsedToday);
            _missionsBtn?.SetEnabled(!isDay);
        }

        void CleanupDestroyedPanels()
        {
            if (_lawPanel == null) _lawsBtn?.RemoveFromClassList("hud-btn--active");
            if (_orderPanel == null) _ordersBtn?.RemoveFromClassList("hud-btn--active");
            if (_missionPanel == null) _missionsBtn?.RemoveFromClassList("hud-btn--active");
        }

        void OnLawsClicked()
        {
            var wasShown = _lawPanel != null;
            CloseAllPanels();
            if (!wasShown)
            {
                _lawPanel = UISystem.Open<GUILawPanel>(UILayer.Window);
                _lawPanel?.Show();
                _lawsBtn?.AddToClassList("hud-btn--active");
            }
        }

        void OnOrdersClicked()
        {
            var wasShown = _orderPanel != null;
            CloseAllPanels();
            if (!wasShown)
            {
                _orderPanel = UISystem.Open<GUIOrderPanel>(UILayer.Window);
                _orderPanel?.Show();
                _ordersBtn?.AddToClassList("hud-btn--active");
            }
        }

        void OnMissionsClicked()
        {
            var wasShown = _missionPanel != null;
            CloseAllPanels();
            if (!wasShown)
            {
                _missionPanel = UISystem.Open<GUIMissionPanel>(UILayer.Window);
                _missionPanel?.Show();
                _missionsBtn?.AddToClassList("hud-btn--active");
            }
        }

        void CloseAllPanels()
        {
            if (_lawPanel != null)
            {
                Destroy(_lawPanel.gameObject);
                _lawPanel = null;
            }

            _lawsBtn?.RemoveFromClassList("hud-btn--active");
            if (_orderPanel != null)
            {
                Destroy(_orderPanel.gameObject);
                _orderPanel = null;
            }

            _ordersBtn?.RemoveFromClassList("hud-btn--active");
            if (_missionPanel != null)
            {
                Destroy(_missionPanel.gameObject);
                _missionPanel = null;
            }

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

        static void SetupActionButtonTooltip(SiegeButton btn, string title, Func<string> descProvider)
        {
            if (btn == null) return;
            var wrapper = btn.parent;
            wrapper.pickingMode = PickingMode.Position;
            wrapper.AddManipulator(new TooltipManipulator(title, descProvider));
        }

        string GetActionButtonDescription(bool isDayAction, string enabledDesc)
        {
            if (_clock == null) return enabledDesc;
            bool isDay = _clock.IsDay;

            if (isDayAction)
            {
                if (!isDay) return "Only available during the day";
                if (_state is { ActionUsedToday: true }) return "Action already used today";
            }

            return enabledDesc;
        }

        string GetMissionsButtonDescription()
        {
            if (_clock == null) return "Launch a covert mission under cover of night";
            if (_clock.IsDay) return "Only available at night";
            return "Launch a covert mission under cover of night";
        }
    }
}