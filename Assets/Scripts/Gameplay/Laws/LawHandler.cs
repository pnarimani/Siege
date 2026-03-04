using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Laws
{
    public abstract class LawHandler<T> : ILawHandler where T : Law
    {
        protected readonly T Law;
        protected readonly IPopupService Popup;

        protected LawHandler(T law, IPopupService popup)
        {
            Law = law;
            Popup = popup;
        }

        public string LawId => Law.Id;
        public abstract bool CanEnact(GameState state);
        public abstract void ApplyImmediate(GameState state, ChangeLog log);
        public virtual void OnDayTick(GameState state, ChangeLog log) { }
    }
}
