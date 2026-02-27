using Unity.Properties;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class SiegeButton : VisualElement
    {
        readonly TextElement _text;
        readonly VisualElement _bg;
        readonly VisualElement _shadow;

        [CreateProperty]
        public virtual string Text
        {
            get => _text.text;
            set => _text.text  = value;
        }
        
        public SiegeButton()
        {
            _text = new TextElement { pickingMode = PickingMode.Ignore };
            _bg = new VisualElement { pickingMode = PickingMode.Ignore };
            _shadow = new VisualElement { pickingMode = PickingMode.Ignore };

            AddToClassList("siege-button");
            AddToClassList("hover-punch");
            _bg.AddToClassList("siege-button-bg");
            _shadow.AddToClassList("siege-button-shadow");
            _text.AddToClassList("siege-button-text");

            Add(_shadow);
            Add(_bg);
            Add(_text);
        }
    }
}