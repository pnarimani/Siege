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

    public class GUIBuildingPanel : MonoBehaviour
    {
        [SerializeField] LocalizedString _enabledString, _disabledString;

        SiegeButton _button;
        StorageViewModel _viewModel;
        VisualElement _root;

        const string ActivatedClass = "building-button--activated";
        const string DeactiveClass = "building-button--disabled";

        void Awake()
        {
            _root = this.FindElement<VisualElement>("Root");
            _button = this.FindElement<SiegeButton>("BuildingToggle");
            _button.Clicked += OnToggleClicked;
            _viewModel = new StorageViewModel();
        }

        void OnToggleClicked()
        {
        }
    }
}