using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    public class LawDispatcher
    {
        readonly List<ILaw> _templates;
        readonly List<ILaw> _enacted = new();
        readonly GameState _state;
        readonly ChangeLog _changeLog;

        public IReadOnlyList<ILaw> AllLaws => _templates;
        public IReadOnlyList<ILaw> EnactedLaws => _enacted;

        public event Action<string> LawEnacted;

        public LawDispatcher(IEnumerable<ILaw> laws, GameState state, ChangeLog changeLog)
        {
            _templates = new List<ILaw>(laws);
            _state = state;
            _changeLog = changeLog;
        }

        public ILaw GetLaw(string id)
        {
            foreach (var law in _templates)
                if (law.Id == id) return law;
            return null;
        }

        public bool IsEnacted(string id) => _state.EnactedLawIds.Contains(id);

        public bool CanEnact(string id)
        {
            if (IsEnacted(id)) return false;
            var law = GetLaw(id);
            return law != null && law.CanEnact(_state);
        }

        public bool TryEnact(string id)
        {
            if (IsEnacted(id)) return false;
            var law = GetLaw(id);
            if (law == null || !law.CanEnact(_state)) return false;

            var copy = law.Clone();
            _state.EnactedLawIds.Add(id);
            copy.OnEnact(_state, _changeLog);
            _enacted.Add(copy);
            LawEnacted?.Invoke(id);
            return true;
        }

        public void TickAll()
        {
            foreach (var law in _enacted)
                law.ApplyDailyEffect(_state, _changeLog);
        }
    }
}
