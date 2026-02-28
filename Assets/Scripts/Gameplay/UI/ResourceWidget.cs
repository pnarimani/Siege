using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class ResourceWidget : VisualElement
    {
        private readonly Image _icon;
        private readonly TextElement _text;

        // Backing fields (what UXML/UI Builder edits conceptually)
        private Sprite _iconValue;
        private string _textValue;

        [UxmlAttribute(name: "icon"), CreateProperty]
        public Sprite Icon
        {
            get => _iconValue;
            set
            {
                if (_iconValue == value) return;
                _iconValue = value;
                ApplyIcon();
            }
        }

        [UxmlAttribute(name: "text"), CreateProperty]
        public string Text
        {
            get => _textValue;
            set
            {
                if (_textValue == value) return;
                _textValue = value;
                ApplyText();
            }
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

            // Important: apply whatever values were deserialized/edited
            ApplyIcon();
            ApplyText();
        }

        private void ApplyIcon()
        {
            if (_icon == null) return;                 // defensive for UI Builder edge cases
            _icon.sprite = _iconValue;
            _icon.MarkDirtyRepaint();
        }

        private void ApplyText()
        {
            if (_text == null) return;                 // defensive for UI Builder edge cases
            _text.text = _textValue ?? string.Empty;
            _text.MarkDirtyRepaint();
        }
    }
}