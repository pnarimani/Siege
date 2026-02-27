using System;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class SiegeButton : VisualElement
    {
        readonly TextElement _text;
        Clickable _clickable;

        [CreateProperty]
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        public Clickable Clickable
        {
            get => _clickable;
            set
            {
                if (_clickable != null && _clickable.target == this)
                    this.RemoveManipulator(_clickable);
                _clickable = value;
                if (_clickable == null)
                    return;
                this.AddManipulator(_clickable);
            }
        }

        public event Action Clicked
        {
            add
            {
                if (_clickable == null)
                    Clickable = new Clickable(value);
                else
                    _clickable.clicked += value;
            }
            remove
            {
                if (_clickable == null)
                    return;
                _clickable.clicked -= value;
            }
        }

        public SiegeButton()
        {
            _text = new TextElement { pickingMode = PickingMode.Ignore };
            var bg = new VisualElement { pickingMode = PickingMode.Ignore };
            var shadow = new VisualElement { pickingMode = PickingMode.Ignore };

            AddToClassList("button");
            AddToClassList("hover-punch");
            bg.AddToClassList("button__bg");
            shadow.AddToClassList("button__shadow");
            _text.AddToClassList("button__text");

            Add(shadow);
            Add(bg);
            Add(_text);
        }
    }
}