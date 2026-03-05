using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Missions
{
    public class SearchAbandonedHomes : IMission
    {
        readonly IPopupService _popup;
        readonly ResourceLedger _ledger;

        const int DurationDays = 2;
        const int Workers = 4;
        const float ChanceMaterials = 0.45f;
        const float ChanceMedicine = 0.35f;
        const double MaterialsGain = 40;
        const double MedicineGain = 25;
        const double SicknessMinor = 5;
        const double SicknessMajor = 15;
        const int FailDeaths = 2;

        const string MaterialsText = "Useful materials recovered, but some workers fell ill from the filth.";
        const string MedicineText = "A hidden apothecary cache. The workers cough but carry on.";
        const string FailText = "The houses were plague-ridden. Two workers never returned.";

        int _daysRemaining;
        int _workersSent;

        public SearchAbandonedHomes(IPopupService popup, ResourceLedger ledger)
        {
            _popup = popup;
            _ledger = ledger;
        }

        public string Id => "search_abandoned_homes";
        public string Name => "Search Abandoned Homes";
        public string Description => "Scour abandoned dwellings for supplies. Duration: 2d | Workers: 4";
        public bool IsComplete => _daysRemaining <= 0;
        public float Progress => 1f - (float)_daysRemaining / DurationDays;

        public bool CanLaunch(GameState state) => state.HealthyWorkers >= Workers;

        public void OnLaunch(GameState state, ChangeLog log)
        {
            _daysRemaining = DurationDays;
            _workersSent = Workers;
            state.HealthyWorkers -= Workers;
            log.Record("HealthyWorkers", -Workers, Name);
        }

        public void AdvanceDay(GameState state, ChangeLog log) => _daysRemaining--;

        public MissionOutcome Complete(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            float roll = Random.value;
            MissionOutcome outcome;

            if (roll < ChanceMaterials)
            {
                _ledger.Deposit(ResourceType.Materials, MaterialsGain);
                state.Sickness += SicknessMinor;
                log.Record("Materials", MaterialsGain, Name);
                log.Record("Sickness", SicknessMinor, Name);
                outcome = new MissionOutcome { NarrativeText = MaterialsText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            if (roll < ChanceMaterials + ChanceMedicine)
            {
                _ledger.Deposit(ResourceType.Medicine, MedicineGain);
                state.Sickness += SicknessMinor;
                log.Record("Medicine", MedicineGain, Name);
                log.Record("Sickness", SicknessMinor, Name);
                outcome = new MissionOutcome { NarrativeText = MedicineText, Success = true };
                ReturnSurvivors(state, log, outcome);
                _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
                return outcome;
            }

            state.Sickness += SicknessMajor;
            state.TotalDeaths += FailDeaths;
            state.DeathsToday += FailDeaths;
            log.Record("Sickness", SicknessMajor, Name);
            log.Record("Deaths", FailDeaths, Name);
            outcome = new MissionOutcome { NarrativeText = FailText, Success = false, Deaths = FailDeaths };
            ReturnSurvivors(state, log, outcome);
            _popup.Open(Name, outcome.NarrativeText, log.SliceSince(before));
            return outcome;
        }

        public void OnCancelled(GameState state, ChangeLog log)
        {
            state.HealthyWorkers += _workersSent;
            log.Record("HealthyWorkers", _workersSent, Name + " (cancelled)");
        }

        public IMission Clone() => new SearchAbandonedHomes(_popup, _ledger);

        void ReturnSurvivors(GameState state, ChangeLog log, MissionOutcome outcome)
        {
            int healthy = System.Math.Max(0, _workersSent - outcome.Deaths - outcome.Wounded);
            if (healthy > 0)
            {
                state.HealthyWorkers += healthy;
                log.Record("HealthyWorkers", healthy, Name + " (returned)");
            }
            if (outcome.Wounded > 0)
            {
                state.SickWorkers += outcome.Wounded;
                log.Record("SickWorkers", outcome.Wounded, Name + " (wounded)");
            }
        }
    }
}
