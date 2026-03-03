using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.LossConditions
{
    public class GameEndState
    {
        public GameOverReason Reason;
        public int DaysSurvived;
        public int PopulationRemaining;
        public int TotalDeaths;
        public int LawsEnacted;
        public int OrdersIssued;
        public int ZonesLost;
        public string NarrativeText;

        public static GameEndState Create(GameState state, GameOverReason reason)
        {
            return new GameEndState
            {
                Reason = reason,
                DaysSurvived = state.CurrentDay,
                PopulationRemaining = state.HealthyWorkers + state.SickWorkers + state.Guards
                                      + state.WoundedGuards + state.Elderly,
                TotalDeaths = state.TotalDeaths,
                LawsEnacted = state.EnactedLawIds.Count,
                OrdersIssued = state.OrdersIssuedCount,
                ZonesLost = state.ZonesLostCount,
                NarrativeText = GetNarrative(reason)
            };
        }

        static string GetNarrative(GameOverReason reason) => reason switch
        {
            GameOverReason.KeepBreached =>
                "The keep has fallen. Enemy soldiers pour through the shattered gates. The siege is over.",
            GameOverReason.CouncilRevolt =>
                "The council has risen against you. Armed retainers seize the halls of power. Your rule ends in bloodshed.",
            GameOverReason.TotalCollapse =>
                "The last rations are gone. The last barrels are dry. People collapse in the streets. The siege claims its final victims.",
            GameOverReason.Victory =>
                "The relief army crashes into the enemy's rear. Horns sound victory. The siege is broken. You survived.",
            _ => ""
        };
    }
}
