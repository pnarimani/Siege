using System.Linq;
using AutofacUnity;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class StorageViewModel
    {
        [UsedImplicitly] [CreateProperty] public string Fuel, Food, Water, Medicine, Materials;
    }

    public class BuildingView : MonoBehaviour
    {
        [SerializeField] LocalizedString _enabledString, _disabledString;

        SiegeButton _button;
        StorageViewModel _viewModel;
        VisualElement _root;

        const string ActivatedClass = "building-button--activated";
        const string DeactiveClass = "building-button--disabled";

        bool _isActivated = true;
        GameBalance _gameBalance;

        void Awake()
        {
            _root = this.FindElement<VisualElement>("Root");
            _button = this.FindElement<SiegeButton>("BuildingToggle");
            _button.Clicked += OnButtonClicked;
            _viewModel = new StorageViewModel();
            
            _gameBalance = Resolver.Resolve<GameBalance>();
        }

        public async void Show(Building building)
        {
            if (building.Id == BuildingId.Storage)
                ShowStorage(building);
            else
                ShowProduction();

            await Awaitable.NextFrameAsync();
            _root.AddToClassList("building-panel--in");
        }

        void ShowProduction()
        {
            this.FindElement<VisualElement>("StorageBuilding").style.display = DisplayStyle.None;
            this.FindElement<VisualElement>("ProductionBuilding").style.display = DisplayStyle.Flex;
            SetToggled(true);
        }

        void ShowStorage(Building building)
        {
            this.FindElement<VisualElement>("ProductionBuilding").style.display = DisplayStyle.None;
            var storageParent = this.FindElement<VisualElement>("StorageBuilding");
            storageParent.style.display = DisplayStyle.Flex;
            Assign(building, out _viewModel.Food, ResourceType.Food);
            Assign(building, out _viewModel.Fuel, ResourceType.Fuel);
            Assign(building, out _viewModel.Water, ResourceType.Water);
            Assign(building, out _viewModel.Medicine, ResourceType.Medicine);
            Assign(building, out _viewModel.Materials, ResourceType.Materials);
            storageParent.dataSource = _viewModel;
        }

        void OnButtonClicked()
        {
            SetToggled(!_isActivated);
        }

        void SetToggled(bool isActivated)
        {
            _isActivated = isActivated;
            if (_isActivated)
            {
                _button.AddToClassList(ActivatedClass);
                _button.RemoveFromClassList(DeactiveClass);
                _button.Text = _enabledString.GetLocalizedString();
            }
            else
            {
                _button.AddToClassList(DeactiveClass);
                _button.RemoveFromClassList(ActivatedClass);
                _button.Text = _disabledString.GetLocalizedString();
            }
        }

        void Assign(Building building, out string field, ResourceType type)
        {
            var x = building.Resources.FirstOrDefault(x => x.Resource == type).Quantity;
            field = $"{x:F0}/{_gameBalance.StorageBaseCapacity}";
        }
    }
}