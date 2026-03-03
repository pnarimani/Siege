using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class ResourceWidget : VisualElement
    {
        private readonly Image _icon;
        private readonly TextElement _quantityText, _labelText;

        [UxmlAttribute(name: "icon"), CreateProperty]
        public Sprite Icon
        {
            get => _icon.sprite;
            set
            {
                if (_icon.sprite == value) return;
                _icon.sprite = value;
                _icon.MarkDirtyRepaint();
            }
        }

        [UxmlAttribute(name: "text"), CreateProperty]
        public string Text
        {
            get => _quantityText.text;
            set
            {
                if (_quantityText.text == value) return;
                _quantityText.text = value;
                _quantityText.MarkDirtyRepaint();
            }
        }

        [UxmlAttribute(name: "label"), CreateProperty]
        public string Label
        {
            get => _labelText.text;
            set
            {
                if (_labelText.text == value) return;
                _labelText.text = value;
                _labelText.MarkDirtyRepaint();
            }
        }

        public ResourceWidget()
        {
            _icon = new Image { pickingMode = PickingMode.Ignore };
            _quantityText = new TextElement { pickingMode = PickingMode.Ignore };
            _labelText = new TextElement { pickingMode = PickingMode.Ignore };

            AddToClassList("resource-widget");
            _icon.AddToClassList("resource-widget__icon");
            _quantityText.AddToClassList("resource-widget__text");
            _labelText.AddToClassList("resource-widget__label");

            Add(_icon);
            Add(_labelText);
            Add(_quantityText);
        }
    }
}