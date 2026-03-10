using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.UI
{
    public class UIToolkitView : MonoBehaviour
    {
        UIDocument _document;
        
        protected UIDocument Document => _document ??= GetComponent<UIDocument>();
        
        protected VisualElement Root => Document.rootVisualElement;
    }
}