using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class BuildingButton : VisualElement
    {
        const string ClassName = "building-button", LabelClassName = "building-button__label", IconClassName = "building-button__icon";
        
        readonly Image _icon;
        readonly Label _label;

        [CreateProperty]
        [UxmlAttribute("icon")]
        public Sprite Icon
        {
            get => _icon.sprite;
            set => _icon.sprite = value;
        }

        [CreateProperty]
        [UxmlAttribute("label")]
        public string Label
        {
            get => _label.text;
            set => _label.text = value;
        }

        public BuildingButton()
        {
            _icon = new Image();
            _label = new Label();
            
            AddToClassList(ClassName);
            _icon.AddToClassList(IconClassName);
            _label.AddToClassList(LabelClassName);

            Add(_icon);
            Add(_label);
        }
    }
}