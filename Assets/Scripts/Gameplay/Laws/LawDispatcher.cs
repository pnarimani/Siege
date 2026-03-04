using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    /// <summary>
    /// Wrapper class for all law operations. External systems interact with this, not individual handlers.
    /// Replaces LawManager.
    /// </summary>
    public class LawDispatcher
    {
        readonly List<ILaw> _laws;
        readonly Dictionary<string, ILawHandler> _handlers;
        readonly GameState _state;
        readonly ChangeLog _changeLog;

        public IReadOnlyList<ILaw> AllLaws => _laws;

        public event Action<string> LawEnacted;

        public LawDispatcher(IEnumerable<ILaw> laws, IEnumerable<ILawHandler> handlers, GameState state, ChangeLog changeLog)
        {
            _laws = new List<ILaw>(laws);
            _handlers = new Dictionary<string, ILawHandler>();
            foreach (var h in handlers)
                _handlers[h.LawId] = h;
            _state = state;
            _changeLog = changeLog;
        }

        public ILaw GetLaw(string id)
        {
            foreach (var law in _laws)
                if (law.Id == id) return law;
            return null;
        }

        public bool IsEnacted(string id) => _state.EnactedLawIds.Contains(id);

        public bool CanEnact(string id)
        {
            if (!_handlers.TryGetValue(id, out var handler)) return false;
            var law = GetLaw(id);
            return law != null && !law.IsEnacted && handler.CanEnact(_state);
        }

        public bool TryEnact(string id)
        {
            if (!_handlers.TryGetValue(id, out var handler)) return false;
            var law = GetLaw(id);
            if (law == null || law.IsEnacted || !handler.CanEnact(_state)) return false;

            law.IsEnacted = true;
            _state.EnactedLawIds.Add(id);
            handler.ApplyImmediate(_state, _changeLog);
            LawEnacted?.Invoke(id);
            return true;
        }

        public void TickAll()
        {
            foreach (var law in _laws)
            {
                if (law.IsEnacted && _handlers.TryGetValue(law.Id, out var handler))
                    handler.OnDayTick(_state, _changeLog);
            }
        }

        public double CombinedProductionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _laws)
                    if (law.IsEnacted) mult *= law.ProductionMultiplier;
                return mult;
            }
        }

        public double CombinedFoodConsumptionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _laws)
                    if (law.IsEnacted) mult *= law.FoodConsumptionMultiplier;
                return mult;
            }
        }

        public double CombinedWaterConsumptionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _laws)
                    if (law.IsEnacted) mult *= law.WaterConsumptionMultiplier;
                return mult;
            }
        }

        public double CombinedSiegeDamageMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _laws)
                    if (law.IsEnacted) mult *= law.SiegeDamageMultiplier;
                return mult;
            }
        }
    }
}
