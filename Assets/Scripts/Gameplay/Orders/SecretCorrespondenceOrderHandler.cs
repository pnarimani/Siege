using Siege.Gameplay;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class SecretCorrespondenceOrderHandler : OrderHandler<SecretCorrespondenceOrder>
    {
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

        readonly PoliticalState _political;

        public SecretCorrespondenceOrderHandler(SecretCorrespondenceOrder order, IPopupService popup, PoliticalState political) : base(order, popup)
        {
            _political = political;
        }

        public override bool CanIssue(GameState state) =>
            _political.Faith.Value >= 4;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            Popup.Open(Order.Name, Order.NarrativeText, log.SliceSince(before));
        }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, Order.Id);

            state.Morale += DailyMoraleGain;
            log.Record("Morale", DailyMoraleGain, Order.Id);

            if (Random.value < ResourceBonusChance)
            {
                var type = Resources[Random.Range(0, Resources.Length)];
                state.AddResource(type, BonusResourceAmount);
                log.Record(type.ToString(), BonusResourceAmount, Order.Id + "_bonus");
            }

            if (Random.value < SignalFireChance)
            {
                state.SignalFireLit = true;
                log.Record("SignalFireLit", 1, Order.Id);
            }
        }
    }
}
