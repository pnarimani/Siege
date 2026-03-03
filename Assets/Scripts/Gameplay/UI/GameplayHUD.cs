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

        GameState _state;
        GameClock _clock;

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
        }

        void Update()
        {
            if (_state == null) return;
            UpdateResources();
            UpdateStatusBars();
        }

        void UpdateResources()
        {
            if (_food != null) _food.Text = ((int)_state.Food).ToString();
            if (_water != null) _water.Text = ((int)_state.Water).ToString();
            if (_fuel != null) _fuel.Text = ((int)_state.Fuel).ToString();
            if (_materials != null) _materials.Text = ((int)_state.Materials).ToString();
            if (_meds != null) _meds.Text = ((int)_state.Medicine).ToString();
        }

        void UpdateStatusBars()
        {
            _morale?.Set01((float)(_state.Morale / 100.0));
            _unrest?.Set01((float)(_state.Unrest / 100.0));
            _sickness?.Set01((float)(_state.Sickness / 100.0));
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