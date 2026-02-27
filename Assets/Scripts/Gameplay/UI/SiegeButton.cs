using Unity.Properties;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class SiegeButton : VisualElement
    {
        readonly TextElement _text;

        [CreateProperty]
        public string Text
        {
            get => _text.text;
            set => _text.text  = value;
        }
        
        public SiegeButton()
        {
            _text = new TextElement {  pickingMode = PickingMode.Ignore };
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