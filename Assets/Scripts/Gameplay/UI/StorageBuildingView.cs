using System.Linq;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class StorageViewModel
    {
        [UsedImplicitly] [CreateProperty] public int Fuel, Food, Water, Medicine, Materials;
    }

    public class StorageBuildingView : MonoBehaviour
    {
        StorageViewModel _viewModel;
        VisualElement _root;

        void Awake()
        {
            _root = this.FindElement<VisualElement>("Root");
            this.FindElement<VisualElement>("ProductionBuilding").style.display = DisplayStyle.None;

            var storageParent = this.FindElement<VisualElement>("StorageBuilding");
            storageParent.style.display = DisplayStyle.Flex;

            _viewModel = new StorageViewModel();

            storageParent.dataSource = _viewModel;
        }

        public async void Show(StorageBuilding building)
        {
            Assign(building, out _viewModel.Food, ResourceType.Food);
            Assign(building, out _viewModel.Fuel, ResourceType.Fuel);
            Assign(building, out _viewModel.Water, ResourceType.Water);
            Assign(building, out _viewModel.Medicine, ResourceType.Medicine);
            Assign(building, out _viewModel.Materials, ResourceType.Materials);

            await Awaitable.NextFrameAsync();
            _root.AddToClassList("building-panel--in");
        }

        void Assign(StorageBuilding building, out int field, ResourceType type)
        {
            field = (int)building.Resources.FirstOrDefault(x => x.Resource == type).Quantity;
        }
    }
}