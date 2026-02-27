using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class ResourceWidget : VisualElement
    {
        readonly Image _icon;
        readonly TextElement _text;

        [CreateProperty]
        public Sprite Icon
        {
            get => _icon.sprite;
            set => _icon.sprite = value;
        }

        [CreateProperty]
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        
        public ResourceWidget()
        {
            _icon = new Image { pickingMode = PickingMode.Ignore };
            _text = new TextElement { pickingMode = PickingMode.Ignore };

            AddToClassList("resource-widget");
            _icon.AddToClassList("resource-widget__icon");
            _text.AddToClassList("resource-widget__text");

            Add(_icon);
            Add(_text);
        }
    }
}