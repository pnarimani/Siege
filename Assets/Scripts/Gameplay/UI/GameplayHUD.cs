using System.Linq;
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
        Building[] _buildings = { };
        float _buildingRefreshTimer;

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

        void Update()
        {
            _buildingRefreshTimer -= Time.deltaTime;
            if (_buildingRefreshTimer <= 0f)
            {
                _buildings = FindObjectsByType<Building>(FindObjectsSortMode.None);
                _buildingRefreshTimer = 2f;
            }

            UpdateResources();
            UpdateStatusBars();
        }

        void UpdateResources()
        {
            if (_food != null) _food.Text = SumResource(ResourceType.Food).ToString();
            if (_water != null) _water.Text = SumResource(ResourceType.Water).ToString();
            if (_fuel != null) _fuel.Text = SumResource(ResourceType.Fuel).ToString();
            if (_materials != null) _materials.Text = SumResource(ResourceType.Materials).ToString();
            if (_meds != null) _meds.Text = SumResource(ResourceType.Medicine).ToString();
        }

        void UpdateStatusBars()
        {
            var gs = GameState.Current;
            if (gs == null) return;
            _morale?.Update01(gs.Morale / 100f);
            _unrest?.Update01(gs.Unrest / 100f);
            _sickness?.Update01(gs.Sickness / 100f);
        }

        int SumResource(ResourceType type) =>
            (int)_buildings
                .Where(b => b != null)
                .Sum(b => b.Resources.FirstOrDefault(r => r.Resource == type).Quantity);

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