using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class SecretCorrespondenceOrder : IOrder
    {
        const string Narrative = "A bird lands on the tower at dawn, a coded message bound to its leg. Hope is a fragile thing.";
        const double DailyMaterialsCost = 4;
        const double DailyMoraleGain = 1;
        const float ResourceBonusChance = 0.08f;
        const double BonusResourceAmount = 4;
        const float SignalFireChance = 0.05f;

        static readonly ResourceType[] Resources =
        {
            ResourceType.Food,
            ResourceType.Water,
            ResourceType.Fuel,
            ResourceType.Medicine,
            ResourceType.Materials
        };

        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public SecretCorrespondenceOrder(IPopupService popup, PoliticalState political)
        {
            _popup = popup;
            _political = political;
        }

        public string Id => "secret_correspondence";
        public string Name => "Secret Correspondence";
        public string Description => "Maintain covert communication with allies outside the walls. May yield supplies or hasten relief.";
        public int CooldownDays => 0;
        public bool IsToggle => true;

        public bool CanIssue(GameState state) =>
            _political.Faith.Value >= 4;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public void ApplyDailyEffect(GameState state, ChangeLog log)
        {
            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, Id);

            state.Morale += DailyMoraleGain;
            log.Record("Morale", DailyMoraleGain, Id);

            if (Random.value < ResourceBonusChance)
            {
                var type = Resources[Random.Range(0, Resources.Length)];
                state.AddResource(type, BonusResourceAmount);
                log.Record(type.ToString(), BonusResourceAmount, Id + "_bonus");
            }

            if (Random.value < SignalFireChance)
            {
                state.SignalFireLit = true;
                log.Record("SignalFireLit", 1, Id);
            }
        }

        public IOrder Clone() => new SecretCorrespondenceOrder(_popup, _political);
    }
}
