using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Laws
{
    /// <summary>
    /// Manages all laws: registration, enactment, and querying.
    /// </summary>
    public class LawManager
    {
        readonly GameState _state;
        readonly ChangeLog _changeLog;
        readonly List<Law> _allLaws = new();

        public IReadOnlyList<Law> AllLaws => _allLaws;

        public LawManager(GameState state, ChangeLog changeLog)
        {
            _state = state;
            _changeLog = changeLog;
            RegisterAllLaws();
        }

        public void Register(Law law) => _allLaws.Add(law);

        public bool TryEnact(string lawId)
        {
            var law = GetLaw(lawId);
            if (law == null || law.IsEnacted || !law.CanEnact(_state)) return false;
            law.Enact(_state, _changeLog);
            return true;
        }

        public Law GetLaw(string id)
        {
            foreach (var law in _allLaws)
                if (law.Id == id) return law;
            return null;
        }

        public bool IsEnacted(string id) => _state.EnactedLawIds.Contains(id);

        /// <summary>
        /// Combined production multiplier from all enacted laws.
        /// </summary>
        public double CombinedProductionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _allLaws)
                    if (law.IsEnacted) mult *= law.ProductionMultiplier;
                return mult;
            }
        }

        public double CombinedFoodConsumptionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _allLaws)
                    if (law.IsEnacted) mult *= law.FoodConsumptionMultiplier;
                return mult;
            }
        }

        public double CombinedWaterConsumptionMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _allLaws)
                    if (law.IsEnacted) mult *= law.WaterConsumptionMultiplier;
                return mult;
            }
        }

        public double CombinedSiegeDamageMultiplier
        {
            get
            {
                double mult = 1.0;
                foreach (var law in _allLaws)
                    if (law.IsEnacted) mult *= law.SiegeDamageMultiplier;
                return mult;
            }
        }

        void RegisterAllLaws()
        {
            Register(new AbandonOuterRingLaw());
            Register(new BurnTheDeadLaw());
            Register(new CannibalismLaw());
            Register(new CollectiveFarmsLaw());
            Register(new ConscriptElderlyLaw());
            Register(new CurfewLaw());
            Register(new EmergencySheltersLaw());
            Register(new ExtendedShiftsLaw());
            Register(new FaithProcessionsLaw());
            Register(new FoodConfiscationLaw());
            Register(new GarrisonMandateLaw());
            Register(new MandatoryGuardServiceLaw());
            Register(new MartialLawLaw());
            Register(new MedicalTriageLaw());
            Register(new OathOfMercyLaw());
            Register(new PublicExecutionsLaw());
            Register(new PurgeTheDisloyalLaw());
            Register(new ScorchedEarthLaw());
            Register(new ShadowCouncilLaw());
            Register(new StrictRationsLaw());
            Register(new WaterRationingLaw());
        }
    }
}
