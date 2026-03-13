using AutofacUnity;
using Siege.UI;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIGameplay : UIToolkitView
    {
        const string BuildButtonName = "build";

        UISystem _uiSystem;

        void Awake()
        {
            _uiSystem = Resolver.Resolve<UISystem>();

            Root.Q<Button>(BuildButtonName).clicked += OpenBuildingPanel;
        }

        void OpenBuildingPanel()
        {
            _uiSystem.Open<GUIBuildingPanel>();
        }
    }
}