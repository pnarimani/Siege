using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Orders
{
    public class BribeEnemyOfficerOrder : Order
    {
        const int Cooldown = 0;
        const double DailyFoodCost = 10;
        const double DailyMaterialsCost = 7;
        const float InterceptChance = 0.10f;
        const double InterceptUnrest = 12;

        public override string Id => "bribe_enemy_officer";
        public override string Name => "Bribe Enemy Officer";
        public override string Description => "Pay a daily tribute to an enemy officer to reduce siege damage. Risk of interception.";
        public override string NarrativeText => "Gold changes hands in the dark. The bombardment eases — but someone may be watching.";
        public override int CooldownDays => Cooldown;
        public override bool IsToggle => true;

        public override bool CanIssue(GameState state) => true;

        public override void Execute(GameState state, ChangeLog log) { }

        public override void OnDayTick(GameState state, ChangeLog log)
        {
            state.Food -= DailyFoodCost;
            log.Record("Food", -DailyFoodCost, Id);

            state.Materials -= DailyMaterialsCost;
            log.Record("Materials", -DailyMaterialsCost, Id);

            // -20% siege damage via temporal modifier
            state.SiegeDamageReductionDays = 1;
            state.SiegeDamageReductionMultiplier = 0.8;

            if (Random.value < InterceptChance)
            {
                state.Unrest += InterceptUnrest;
                log.Record("Unrest", InterceptUnrest, Id + "_intercepted");
            }
        }
    }
}
