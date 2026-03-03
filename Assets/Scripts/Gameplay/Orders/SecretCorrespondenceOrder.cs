using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class SecretCorrespondenceOrder : Order
    {
        const int Cooldown = 0;
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

        public override string Id => "secret_correspondence";
        public override string Name => "Secret Correspondence";
        public override string Description => "Maintain covert communication with allies outside the walls. May yield supplies or hasten relief.";
        public override string NarrativeText => "A bird lands on the tower at dawn, a coded message bound to its leg. Hope is a fragile thing.";
        public override int CooldownDays => Cooldown;
        public override bool IsToggle => true;

        public override bool CanIssue(GameState state) =>
            true; // TODO: require Faith >= 4

        public override void Execute(GameState state, ChangeLog log) { }

        public override void OnDayTick(GameState state, ChangeLog log)
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
    }
}
