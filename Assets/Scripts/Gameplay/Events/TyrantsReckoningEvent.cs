using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class TyrantsReckoningEvent : GameEvent
    {
        public override string Id => "tyrants_reckoning";
        public override string Name => "The Tyrant's Reckoning";
        public override string Description => "The people have had enough. A delegation arrives at the council chamber — not asking, but demanding change.";
        public override bool IsOneTime => true;
        public override int Priority => 75;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state)
        {
            return state.CurrentDay >= 20
                && Resolver.Resolve<PoliticalState>().Tyranny.Value >= 8;
        }

        public override EventResponse[] GetResponses(GameState state)
        {
            return new[]
            {
                new EventResponse(
                    "Double down",
                    "+1 Tyranny, -30 Morale. Crush all dissent."),
                new EventResponse(
                    "Show mercy",
                    "-20 Unrest, +15 Morale, +2 Faith, -3 Tyranny. A rare olive branch.")
            };
        }

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            var p = Resolver.Resolve<PoliticalState>();

            switch (responseIndex)
            {
                case 0: // Double Down
                    p.Tyranny.Add(1);
                    state.Morale -= 30;
                    log.Record("Morale", -30, Name);
                    break;

                case 1: // Show Mercy
                    state.Unrest -= 20;
                    state.Morale += 15;
                    p.Faith.Add(2);
                    p.Tyranny.Add(-3);
                    log.Record("Unrest", -20, Name);
                    log.Record("Morale", 15, Name);
                    break;
            }
        }

        public override string GetNarrativeText(GameState state) => Description;
    }
}
