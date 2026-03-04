using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Missions
{
    public abstract class MissionHandler<T> : IMissionHandler where T : IMission
    {
        protected readonly T Mission;
        protected readonly IPopupService Popup;

        protected MissionHandler(T mission, IPopupService popup)
        {
            Mission = mission;
            Popup = popup;
        }

        public string MissionId => Mission.Id;
        public abstract bool CanLaunch(GameState state);
        public abstract MissionOutcome Resolve(GameState state, ChangeLog log);
    }
}
