using Siege.Gameplay;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class SecretCorrespondenceOrderHandler : IOrderHandler
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

        readonly SecretCorrespondenceOrder _order;
        readonly IPopupService _popup;
        readonly PoliticalState _political;

        public SecretCorrespondenceOrderHandler(SecretCorrespondenceOrder order, IPopupService popup, PoliticalState political)
        {
            _order = order;
            _popup = popup;
            _political = political;
        }

        public string OrderId => _order.Id;

        public bool CanIssue(GameState state) =>
            _political.Faith.Value >= 4;

        public void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            _popup.Open(_order.Name, _order.NarrativeText, log.SliceSince(before));
        }

        public void OnDayTick(GameState state, ChangeLog log)
        {
            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, _order.Id);

            state.Morale += DailyMoraleGain;
            log.Record("Morale", DailyMoraleGain, _order.Id);

            if (Random.value < ResourceBonusChance)
            {
                var type = Resources[Random.Range(0, Resources.Length)];
                state.AddResource(type, BonusResourceAmount);
                log.Record(type.ToString(), BonusResourceAmount, _order.Id + "_bonus");
            }

            if (Random.value < SignalFireChance)
            {
                state.SignalFireLit = true;
                log.Record("SignalFireLit", 1, _order.Id);
            }
        }
    }
}
