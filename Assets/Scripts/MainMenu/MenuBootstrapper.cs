using Autofac;
using Siege.UI;

namespace Siege.MainMenu
{
    public class MenuBootstrapper : IStartable
    {
        readonly UISystem _uiSystem;

        public MenuBootstrapper(UISystem uiSystem)
        {
            _uiSystem = uiSystem;
        }

        public void Start()
        {
            _uiSystem.Open<GUIMainMenu>();
        }
    }
}