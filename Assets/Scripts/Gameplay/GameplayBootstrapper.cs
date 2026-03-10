using Autofac;
using Siege.Gameplay.UI;
using Siege.UI;

namespace Siege.Gameplay
{
    public class GameplayBootstrapper : IStartable
    {
        readonly UISystem _uiSystem;

        public GameplayBootstrapper(UISystem uiSystem)
        {
            _uiSystem = uiSystem;
        }

        public void Start()
        {
            _uiSystem.Open<GUIGameplay>();
        }
    }
}