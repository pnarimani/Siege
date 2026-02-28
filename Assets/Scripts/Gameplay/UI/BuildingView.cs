using UnityEngine;
using UnityEngine.Localization;

namespace Siege.Gameplay.UI
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] LocalizedString _enabledString, _disabledString;

        SiegeButton _button;
        const string ActivatedClass = "building-button--activated";
        const string DeactiveClass = "building-button--disabled";

        bool _isActivated = true;

        void Awake()
        {
            _button = this.FindElement<SiegeButton>("BuildingToggle");
            _button.Clicked += OnButtonClicked;
            SetToggled(true);
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
    }
}